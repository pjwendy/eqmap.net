# OpenEQ.Netcode.GroupMember Namespace

## Methods

### #ctor(System.String)



**Parameters:**

- $paramName: The name of the group member.

### #ctor(System.Byte[],System.Int32)



**Parameters:**

- $paramName: The byte array containing the group member data.
- $paramName: The offset in the array to start reading from.

### #ctor(System.IO.BinaryReader)



**Parameters:**

- $paramName: The to read the group member data from.

### Unpack(System.Byte[],System.Int32)

Unpacks the group member data from a byte array.

**Parameters:**

- $paramName: The byte array containing the group member data.
- $paramName: The offset in the array to start reading from.

### Unpack(System.IO.BinaryReader)



**Parameters:**

- $paramName: The to read the group member data from.

### Pack

Packs the group member data into a byte array.

**Returns:** A byte array containing the packed group member data.

### Pack(System.IO.BinaryWriter)



**Parameters:**

- $paramName: The to write the group member data to.

### ToString



**Returns:** A byte array containing the packed group member data.


