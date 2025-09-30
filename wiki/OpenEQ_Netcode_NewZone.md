# OpenEQ.Netcode.NewZone Namespace

## Methods

### #ctor(System.Byte[],System.Byte[],System.Byte[],System.Byte,System.Byte[],System.Byte[],System.Byte[],System.Byte,System.Single[],System.Single[],System.Single,System.Byte,System.Byte[],System.Byte[],System.Byte[],System.Byte[],System.Byte[],System.Byte,System.Byte[],System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Byte[],System.Byte[],System.Int32,System.Byte[],System.Int32,System.Int32,System.UInt16,System.UInt16,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Single,System.UInt32,System.UInt32,System.UInt32,System.Byte[])

Initializes a new instance of the NewZone struct with specified field values.

**Parameters:**

- $paramName: The charname value.
- $paramName: The zoneshortname value.
- $paramName: The zonelongname value.
- $paramName: The ztype value.
- $paramName: The fogred value.
- $paramName: The foggreen value.
- $paramName: The fogblue value.
- $paramName: The unknown323 value.
- $paramName: The fogminclip value.
- $paramName: The fogmaxclip value.
- $paramName: The gravity value.
- $paramName: The timetype value.
- $paramName: The rainchance value.
- $paramName: The rainduration value.
- $paramName: The snowchance value.
- $paramName: The snowduration value.
- $paramName: The unknown537 value.
- $paramName: The sky value.
- $paramName: The unknown571 value.
- $paramName: The zoneexpmultiplier value.
- $paramName: The safey value.
- $paramName: The safex value.
- $paramName: The safez value.
- $paramName: The minz value.
- $paramName: The maxz value.
- $paramName: The underworld value.
- $paramName: The minclip value.
- $paramName: The maxclip value.
- $paramName: The unknown620 value.
- $paramName: The zoneshortname2 value.
- $paramName: The unknown800 value.
- $paramName: The unknown804 value.
- $paramName: The unknown844 value.
- $paramName: The unknown848 value.
- $paramName: The zoneid value.
- $paramName: The zoneinstance value.
- $paramName: The scriptnpcreceivedanitem value.
- $paramName: The bcheck value.
- $paramName: The scriptidsomething value.
- $paramName: The underworldteleportindex value.
- $paramName: The scriptidsomething3 value.
- $paramName: The suspendbuffs value.
- $paramName: The lavadamage value.
- $paramName: The minlavadamage value.
- $paramName: The unknown888 value.
- $paramName: The unknown889 value.
- $paramName: The unknown890 value.
- $paramName: The unknown891 value.
- $paramName: The unknown892 value.
- $paramName: The unknown893 value.
- $paramName: The falldamage value.
- $paramName: The unknown895 value.
- $paramName: The fastregenhp value.
- $paramName: The fastregenmana value.
- $paramName: The fastregenendurance value.
- $paramName: The unknown908 value.
- $paramName: The unknown912 value.
- $paramName: The fogdensity value.
- $paramName: The unknown920 value.
- $paramName: The unknown924 value.
- $paramName: The unknown928 value.
- $paramName: The unknown932 value.

### #ctor(System.Byte[],System.Int32)

Initializes a new instance of the NewZone struct from binary data.

**Parameters:**

- $paramName: The binary data to unpack.
- $paramName: The offset in the data to start unpacking from.

### #ctor(System.IO.BinaryReader)

Initializes a new instance of the NewZone struct from a BinaryReader.

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


