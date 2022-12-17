# Hardware

This directory contains the schematic and PCB project for the 6502tor hardware. The 6502tor board was designed in KiCAD, an EDA package that is free and open source. If you want to play with the CAD project, you can download KiCAD here:

 - [KiCAD Project Website](https://www.kicad.org/download/)

You can find the project in the `kicad` directory, but if you simply want to view the schematic you can find it in [the PDF in the same directory as this readme](6502tor_schematic.pdf)

## Caveats/Errata

Note that what is included here is the raw version of the project that was used to generate the first revision boards as shipped, and as such both include a handful of errors that are corrected on the shipped hardware with bodge wires:

 - Somehow KiCAD did not indicate to me that I managed to build two separate, unconnected ground nets. This can be fixed by making a connection between the ground pin (pin 10) on the user register IC (the 74574 next to the USB port) and the ground pin (pin 7) of any of the 7400 or 7404 chips in the glue logic section immediately below.

 - The order of the wires connecting the 74244 outputs back to the top three bits of the 74245 data tranceiver is reversed in the design from what it should be. This can be fixed by cutting the three traces that run just above the SMD capacitor located immediately to the left of the right-most 74245 (just above the DIP switches) and then connecting pins 2, 3, and 4 of that IC to pins 12, 14, and 16 of the adjacent 74244.

 - Bizarrely, there's absolutely no connection between the address bit 7 LED and its current-limiting resistor. The fix for this is to connect them.