# OpenEQ.Netcode.Fling Namespace

## Methods

### #ctor(System.UInt32,System.Int32,System.Byte,System.Byte,System.Byte[],System.Single,System.Single,System.Single,System.Single)

Initializes a new instance of the Fling struct with specified field values.

**Parameters:**

- $paramName: The collision value.
- $paramName: The traveltime value.
- $paramName: The unk3 value.
- $paramName: The disablefalldamage value.
- $paramName: The padding value.
- $paramName: The speedz value.
- $paramName: The newy value.
- $paramName: The newx value.
- $paramName: The newz value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the Fling struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the Fling struct from a BinaryReader.

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


