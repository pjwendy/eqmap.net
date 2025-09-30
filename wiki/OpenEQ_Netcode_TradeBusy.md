# OpenEQ.Netcode.TradeBusy Namespace

## Methods

### #ctor(System.UInt32,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte)

Initializes a new instance of the TradeBusy struct with specified field values.

**Parameters:**

- $paramName: The tomobid value.
- $paramName: The frommobid value.
- $paramName: The type value.
- $paramName: The unknown09 value.
- $paramName: The unknown10 value.
- $paramName: The unknown11 value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the TradeBusy struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the TradeBusy struct from a BinaryReader.

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


