# OpenEQ.Netcode.ClientUpdateFromServer Namespace

## Methods

### #ctor(System.UInt16,OpenEQ.Netcode.UpdatePositionFromServer)

Initializes a new ClientUpdateFromServer with the specified ID and position data.

**Parameters:**

- $paramName: The spawn ID of the entity
- $paramName: The position and movement data

### #ctor(System.Byte[],System.Int32)

Initializes a new ClientUpdateFromServer by unpacking data from a byte array.

**Parameters:**

- $paramName: The binary data to unpack
- $paramName: The offset in the data array to start unpacking from

### #ctor(System.IO.BinaryReader)

Initializes a new ClientUpdateFromServer by unpacking data from a BinaryReader.

**Parameters:**

- $paramName: The BinaryReader to read data from


