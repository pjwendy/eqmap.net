# OpenEQ.Netcode.SpellEffect Namespace

## Methods

### #ctor(System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Byte,System.Byte,System.UInt16)

Initializes a new instance of the SpellEffect struct with specified field values.

**Parameters:**

- $paramName: The effectid value.
- $paramName: The entityid value.
- $paramName: The entityid2 value.
- $paramName: The duration value.
- $paramName: The finishdelay value.
- $paramName: The unknown020 value.
- $paramName: The unknown024 value.
- $paramName: The unknown025 value.
- $paramName: The unknown026 value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the SpellEffect struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the SpellEffect struct from a BinaryReader.

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


