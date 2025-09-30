# OpenEQ.Netcode.ZonePlayerToBind Namespace

## Methods

### #ctor(System.UInt16,System.UInt16,System.Single,System.Single,System.Single,System.Single,System.Byte,System.Byte,System.UInt32,System.UInt32,System.UInt32)

Initializes a new instance of the ZonePlayerToBind struct with specified field values.

**Parameters:**

- $paramName: The bindzoneid value.
- $paramName: The bindinstanceid value.
- $paramName: The x value.
- $paramName: The y value.
- $paramName: The z value.
- $paramName: The heading value.
- $paramName: The zonename value.
- $paramName: The unknown021 value.
- $paramName: The unknown022 value.
- $paramName: The unknown023 value.
- $paramName: The unknown024 value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the ZonePlayerToBind struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the ZonePlayerToBind struct from a BinaryReader.

**Parameters:**

- $paramName: The BinaryReader to read data from.

### Unpack(System.Byte[],System.Int32)

Unpacks the struct data from a byte array.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### Unpack(System.IO.BinaryReader)

Unpacks the struct data from a BinaryReader.

**Parameters:**

- $paramName: The BinaryReader to read data from.

### Pack

Packs the struct data into a byte array.

**Returns:** A byte array containing the packed struct data.

### Pack(System.IO.BinaryWriter)

Packs the struct data into a BinaryWriter.

**Parameters:**

- $paramName: The BinaryWriter to write data to.

### ToString

Returns a string representation of the struct with all field values.

**Returns:** A formatted string containing all field names and values.


