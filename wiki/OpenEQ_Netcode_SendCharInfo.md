# OpenEQ.Netcode.SendCharInfo Namespace

## Methods

### #ctor(System.Collections.Generic.List{EQProtocol.Streams.World.CharacterSelectEntry})



**Parameters:**

- $paramName: The list of character entries.

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

Packs the packet data into a byte array.

**Returns:** A byte array containing the packed data.

### Pack(System.IO.BinaryWriter)



**Parameters:**

- $paramName: The binary writer to write the packet data to.

### ToString



**Returns:** A string describing the contents of the struct.


