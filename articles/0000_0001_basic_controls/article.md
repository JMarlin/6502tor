-- **WIP** --
The basic system consists of a 6502 CPU, a 32-kilobyte RAM, an 8-bit 'user register' for output, and a set of toggle switches and buttons that are used to load code and data into memory, switch between data entry/run mode, and control the CPU execution speed.

This is modeled off of 'full size' computers (as opposed to microcomputers) of the 50s through 70s -- back when there weren't any very good ROM technologies and the machine would be bootstrapped by loading initial code into RAM via one of these 'blinkenlights front panels' -- for more info on their history, check out [this wiki article](https://en.wikipedia.org/wiki/Front_panel). 

The 6502tor is built around a 6502 CPU, a type of 8-bit CPU that's been in production since 1975 and was used in a number of notable machines throughout the 80s including the Apple II, Commodore 64, BBC Micro, and even the Nintendo Entertainment System and Atari 2600.

A CPU communicates with the outside world by loading and storing commands and data between other devices that are connected to it. This loading and storing is done using a pair of busses called the `address bus` and the `data bus` along with a set of `control signals` ('bus' is just a fancy word for a bunch of related wires). The address bus tells connected devices *where* data should be read or written from/to, the data bus carries the actual data itself, and finally the control signals tell the connected devices *when and how* to perform the transfer.

Devices that connect to the CPU 