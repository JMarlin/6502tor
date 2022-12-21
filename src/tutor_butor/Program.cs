using System.IO.Ports;

/* First stage bootloader:
LOC   CODE         LABEL     INSTRUCTION

0200                         * = $0200
0200  A2 00                  LDX #$00
0202               NEXT_BYT  
0202  A9 00                  LDA #$00
0204               WAIT_HI   
0204  2C 00 80               BIT $8000
0207  50 FB                  BVC $0204
0209  10 01                  BPL $020C
020B  38                     SEC
020C               SKIP      
020C  2A                     ROL A
020D               WAIT_LO   
020D  2C 00 80               BIT $8000
0210  70 FB                  BVS $020D
0212  90 F0                  BCC $0204
0214  9D 00 03               STA $0300,X
0217  8D 00 80               STA $8000
021A  E8                     INX
021B  D0 E5                  BNE $0202
021D  4C 00 03               JMP $0300
*/


if(args == null || (args.Count() != 1 && args.Count() != 2)) {

    Console.WriteLine("Please specify a payload file path and optionally a port name");

    return;
}

FileStream payloadStream;

try {
    payloadStream = File.OpenRead(args[0]);
} catch {

    Console.WriteLine($"Unable to open file: {args[0]}");

    return;
}

var port = new SerialPort(
    args.Length == 2
        ? args[1]
        : GetPortFromUserSelection());

var byteBuffer = new byte[256];

payloadStream.Read(byteBuffer, 0, 256);

var intBuffer = byteBuffer.Select(b => (int)b).ToArray();

port.Open();
port.DtrEnable = true;
port.RtsEnable = true;

Console.WriteLine("Ready to send payload. Reset CPU and press enter to continue.");
Console.ReadKey();

Console.WriteLine("Beginning transfer");

var transferIndex = 0;
var startTime = DateTime.Now;

foreach(var data in intBuffer) {

    Console.WriteLine($"    [{++transferIndex}/256]: 0x{data:X2}");

    //Send terminating bit (this will fall into the carry flag when it's rotated out the end of the A register)
    port.DtrEnable = false;

    //Clock
    port.RtsEnable = false;
    port.RtsEnable = true;

    var send = data;

    //Clock out 8 bits of current value from MSB to LSB
    foreach(var idx in Enumerable.Range(0, 8)) {

        //Set next bit value
        port.DtrEnable = (send & 0x80) == 0;
        send = send << 1;

        //Clock
        port.RtsEnable = false;
        port.RtsEnable = true;
    }
}

port.RtsEnable = true;
port.DtrEnable = true;

var totalTime = DateTime.Now - startTime;

Console.WriteLine($"Transfer complete in {totalTime.TotalMilliseconds}ms!");

string GetPortFromUserSelection() {

    var portNames = SerialPort.GetPortNames();

    var portsWithNumbers = portNames
        .Zip(Enumerable.Range(1, portNames.Length));

    Console.WriteLine("Detected the following serial ports:");

    var selectedPortNumber = 0;

    foreach(var portWithNumber in portsWithNumbers)
        Console.WriteLine($"    {portWithNumber.Second}) {portWithNumber.First}");

    Console.Write("Enter a number to use: ");

    while(selectedPortNumber == 0) {
        
        if(!Int32.TryParse(Console.ReadLine(), out selectedPortNumber)
            || selectedPortNumber < 1
            || selectedPortNumber > portNames.Length
        ) {

            Console.Write($"Please enter a number between 1 and {portNames.Length}: ");
            selectedPortNumber = 0;

            continue;
        }
    }

    return portsWithNumbers.First(pwn => pwn.Second == selectedPortNumber).First;
}
