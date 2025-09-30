# OpenEQ.Netcode.SendAATable Namespace

## Methods

### #ctor(System.UInt32,System.Byte,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Byte,System.Byte,System.Byte,System.UInt32,System.UInt32,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte,System.UInt32,System.UInt32)

Initializes a new instance of the SendAATable struct with specified field values.

**Parameters:**

- $paramName: The id value.
- $paramName: The unknown004 value.
- $paramName: The hotkeysid value.
- $paramName: The hotkeysid2 value.
- $paramName: The titlesid value.
- $paramName: The descsid value.
- $paramName: The classtype value.
- $paramName: The cost value.
- $paramName: The seq value.
- $paramName: The currentlevel value.
- $paramName: The prereqskill value.
- $paramName: The prereqminpoints value.
- $paramName: The type value.
- $paramName: The spellid value.
- $paramName: The spelltype value.
- $paramName: The spellrefresh value.
- $paramName: The classes value.
- $paramName: The maxlevel value.
- $paramName: The lastid value.
- $paramName: The nextid value.
- $paramName: The cost2 value.
- $paramName: The unknown81 value.
- $paramName: The grantonly value.
- $paramName: The unknown83 value.
- $paramName: The expendablecharges value.
- $paramName: The aaexpansion value.
- $paramName: The specialcategory value.
- $paramName: The shroud value.
- $paramName: The unknown97 value.
- $paramName: The resetondeath value.
- $paramName: The unknown99 value.
- $paramName: The totalabilities value.
- $paramName: The abilities value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the SendAATable struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the SendAATable struct from a BinaryReader.

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


