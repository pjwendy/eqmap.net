# EQProtocol.Streams.Common Namespace

## Types

### EQStream

Abstract base class for EverQuest network streams that handles reliable UDP communication.
            Provides packet sequencing, acknowledgment, compression, validation, and event emission capabilities.

### IEQStruct

Core interface for all EverQuest packet structures that can be serialized to/from binary data.
            Provides methods for packing and unpacking data structures used in EverQuest network communication.

### Packet

Represents a network packet used in EverQuest protocol communication.
            Handles packet parsing, compression, encryption, and sequencing for reliable network transmission.


