# OpenEQ.Netcode.Utility Namespace

## Methods

### Hexdump(EQProtocol.Streams.Common.AppPacket)

Outputs a formatted hexadecimal dump of a packet to the debug log.
            Includes opcode information and formatted hex/ASCII output.

**Parameters:**

- $paramName: The packet to dump

### Hexdump(System.Byte[])

Outputs a formatted hexadecimal dump of raw data to the debug log.
            Provides hex and ASCII representation for debugging binary data.

**Parameters:**

- $paramName: The byte array to dump

### HexdumpServerStyle(System.Byte[])

Outputs a formatted hexadecimal dump in server log style.
            Matches the formatting used by EQEmu server logs for consistency.

**Parameters:**

- $paramName: The byte array to dump


