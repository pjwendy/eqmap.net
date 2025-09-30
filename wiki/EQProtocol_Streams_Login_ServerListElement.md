# EQProtocol.Streams.Login.ServerListElement Namespace

## Methods

### #ctor(System.String,System.UInt32,System.UInt32,System.String,System.String,System.String,EQProtocol.Streams.Login.ServerStatus,System.UInt32)



**Parameters:**

- $paramName: The IP address of the world server.
- $paramName: The unique ID of the server in the server list.
- $paramName: The runtime ID of the server.
- $paramName: The long name of the server.
- $paramName: The language of the server.
- $paramName: The region of the server.
- $paramName: The status of the server.
- $paramName: The number of players currently online.

### #ctor(System.Byte[],System.Int32)



**Parameters:**

- $paramName: The byte array containing the packed data.
- $paramName: The offset in the array to start reading from.

### #ctor(System.IO.BinaryReader)



**Parameters:**

- $paramName: The binary reader to read from.

### Unpack(System.Byte[],System.Int32)

No description available.

### Unpack(System.IO.BinaryReader)

No description available.

### Pack

No description available.

### Pack(System.IO.BinaryWriter)

No description available.

### ToString



**Returns:** A string describing the server list element.


