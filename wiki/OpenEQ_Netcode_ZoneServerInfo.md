# OpenEQ.Netcode.ZoneServerInfo Namespace

## Methods

### #ctor(System.String,System.UInt16)



**Parameters:**

- $paramName: The IP address or hostname.
- $paramName: The port number.

### #ctor(System.Byte[],System.Int32)



**Parameters:**

- $paramName: The byte array containing the packed data.
- $paramName: The offset in the array to start reading from.

### #ctor(System.IO.BinaryReader)



**Parameters:**

- $paramName: The binary reader containing the packed data.

### Unpack(System.Byte[],System.Int32)

Unpacks the structure from a byte array.

**Parameters:**

- $paramName: The byte array containing the packed data.
- $paramName: The offset in the array to start reading from.

### Unpack(System.IO.BinaryReader)



**Parameters:**

- $paramName: The binary reader containing the packed data.

### Pack

Packs the structure into a byte array.

**Returns:** A byte array containing the packed data.

### Pack(System.IO.BinaryWriter)



**Parameters:**

- $paramName: The binary writer to write the packed data to.

### ToString



**Returns:** A string describing the contents of the structure.


