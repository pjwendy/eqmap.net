# OpenEQ.Netcode.LevelAppearance Namespace

## Methods

### #ctor(System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32)

Initializes a new instance of the LevelAppearance struct with specified field values.

**Parameters:**

- $paramName: The spawnid value.
- $paramName: The parm1 value.
- $paramName: The value1a value.
- $paramName: The value1b value.
- $paramName: The parm2 value.
- $paramName: The value2a value.
- $paramName: The value2b value.
- $paramName: The parm3 value.
- $paramName: The value3a value.
- $paramName: The value3b value.
- $paramName: The parm4 value.
- $paramName: The value4a value.
- $paramName: The value4b value.
- $paramName: The parm5 value.
- $paramName: The value5a value.
- $paramName: The value5b value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the LevelAppearance struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the LevelAppearance struct from a BinaryReader.

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


