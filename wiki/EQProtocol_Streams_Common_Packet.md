# EQProtocol.Streams.Common.Packet Namespace

## Methods

### #ctor(System.UInt16,System.Byte[],System.Boolean)

Initializes a new packet with the specified opcode and data.

**Parameters:**

- $paramName: The packet opcode
- $paramName: The packet data payload
- $paramName: Whether this is a bare packet without session protocol wrapper

### #ctor(EQProtocol.Streams.Common.SessionOp,System.Byte[],System.Boolean)

Initializes a new packet with the specified session operation and data.

**Parameters:**

- $paramName: The session operation type
- $paramName: The packet data payload
- $paramName: Whether this is a bare packet without session protocol wrapper

### #ctor(EQProtocol.Streams.Common.EQStream,System.Byte[],System.Boolean)

Initializes a new packet by parsing raw network data from an EQStream.
            Handles decompression, CRC validation, sequence extraction, and protocol parsing.

**Parameters:**

- $paramName: The EQStream containing session configuration (compression, validation, etc.)
- $paramName: The raw packet data received from the network
- $paramName: Whether this packet is part of a combined packet sequence


