# EQProtocol.Streams.Common.IEQStruct Namespace

## Methods

### Unpack(System.Byte[],System.Int32)

Unpacks binary data into the structure starting at the specified offset.

**Parameters:**

- $paramName: The binary data array containing the packed structure
- $paramName: The byte offset within the data array to start unpacking from (default: 0)

### Unpack(System.IO.BinaryReader)

Unpacks binary data into the structure using a BinaryReader.
            This method provides more advanced reading capabilities and proper endianness handling.

**Parameters:**

- $paramName: The BinaryReader positioned at the start of the structure data

### Pack

Packs the structure into a binary byte array suitable for network transmission.

**Returns:** A byte array containing the packed structure data

### Pack(System.IO.BinaryWriter)

Packs the structure into binary format using a BinaryWriter.
            This method provides more advanced writing capabilities and proper endianness handling.

**Parameters:**

- $paramName: The BinaryWriter to write the structure data to


