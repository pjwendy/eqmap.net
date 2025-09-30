# EQProtocol.Streams.Common.EQStream Namespace

## Methods

### GetOpcodeNameForLogging(System.UInt16)

Gets the opcode name for logging purposes. Override in derived classes for better opcode names.

**Parameters:**

- $paramName: The opcode to get the name for

**Returns:** A string representation of the opcode for logging

### #ctor(System.String,System.Int32)

Initializes a new EQStream with the specified host and port.
            Starts background tasks for packet checking and receiving.

**Parameters:**

- $paramName: The hostname or IP address to connect to
- $paramName: The port number to connect to

### SetPacketEmitter(OpenEQ.Netcode.GameClient.Events.IPacketEventEmitter)

Sets the packet event emitter for this stream, used for logging and debugging packets.

**Parameters:**

- $paramName: The packet event emitter to use, or null to disable packet events


