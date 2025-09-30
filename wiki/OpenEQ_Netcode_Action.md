# OpenEQ.Netcode.Action Namespace

## Methods

### #ctor(System.UInt16,System.UInt16,System.UInt16,System.UInt32,System.Single,System.Single,System.Single,System.Single,System.Byte,System.UInt32,System.UInt16,System.UInt16,System.Byte,System.Byte,System.Byte,System.Byte[],System.UInt32[],System.UInt32)

Initializes a new instance of the Action struct with specified field values.

**Parameters:**

- $paramName: The target value.
- $paramName: The source value.
- $paramName: The level value.
- $paramName: The unknown06 value.
- $paramName: The instrumentmod value.
- $paramName: The force value.
- $paramName: The hitheading value.
- $paramName: The hitpitch value.
- $paramName: The type value.
- $paramName: The damage value.
- $paramName: The unknown31 value.
- $paramName: The spell value.
- $paramName: The spelllevel value.
- $paramName: The effectflag value.
- $paramName: The spellgem value.
- $paramName: The padding38 value.
- $paramName: The slot value.
- $paramName: The itemcasttype value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the Action struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the Action struct from a BinaryReader.

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


