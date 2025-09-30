# EQProtocol.Streams.World.Packets.EnterWorld Namespace

## Methods

### #ctor(System.String,System.Boolean,System.Boolean)



**Parameters:**

- $paramName: The character name.
- $paramName: Whether to enter the tutorial.
- $paramName: Whether to go home.

### #ctor(System.Byte[],System.Int32)



**Parameters:**

- $paramName: The byte array containing the packet data.
- $paramName: The offset in the array to start reading from.

### #ctor(System.IO.BinaryReader)



**Parameters:**

- $paramName: The binary reader containing the packet data.

### Unpack(System.Byte[],System.Int32)

Unpacks the packet data from a byte array.

**Parameters:**

- $paramName: The byte array containing the packet data.
- $paramName: The offset in the array to start reading from.

### Unpack(System.IO.BinaryReader)



**Parameters:**

- $paramName: The binary reader containing the packet data.

### Pack

Packs the current structure into a byte array.

**Returns:** A byte array containing the packed data.

### Pack(System.IO.BinaryWriter)



**Parameters:**

- $paramName: The binary writer to write the packed data to.

### ToString



**Returns:** A string describing the structure.


