# OpenEQ.Netcode.SomeItemPacketMaybe Namespace

## Methods

### #ctor(System.Single,System.Single,System.Single,System.Byte[],System.Single,System.Single,System.Single,System.Byte[],System.Single,System.UInt32,System.UInt32,System.UInt32,System.Byte[],System.Byte,System.Byte,System.Byte,System.Byte[],System.Byte[])

Initializes a new instance of the SomeItemPacketMaybe struct with specified field values.

**Parameters:**

- $paramName: The srcy value.
- $paramName: The srcx value.
- $paramName: The srcz value.
- $paramName: The unknown012 value.
- $paramName: The velocity value.
- $paramName: The launchangle value.
- $paramName: The tilt value.
- $paramName: The unknown036 value.
- $paramName: The arc value.
- $paramName: The sourceid value.
- $paramName: The targetid value.
- $paramName: The itemid value.
- $paramName: The unknown060 value.
- $paramName: The unknown070 value.
- $paramName: The itemtype value.
- $paramName: The skill value.
- $paramName: The unknown073 value.
- $paramName: The modelname value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the SomeItemPacketMaybe struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the SomeItemPacketMaybe struct from a BinaryReader.

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


