# OpenEQ.Netcode.DzExpeditionInvite Namespace

## Methods

### #ctor(System.UInt32,System.UInt32,System.Byte[],System.Byte[],System.Byte,System.Byte[],System.Byte[],System.UInt16,System.UInt16)

Initializes a new instance of the DzExpeditionInvite struct with specified field values.

**Parameters:**

- $paramName: The clientid value.
- $paramName: The unknown004 value.
- $paramName: The invitername value.
- $paramName: The expeditionname value.
- $paramName: The swapping value.
- $paramName: The swapname value.
- $paramName: The padding value.
- $paramName: The dzzoneid value.
- $paramName: The dzinstanceid value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the DzExpeditionInvite struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the DzExpeditionInvite struct from a BinaryReader.

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


