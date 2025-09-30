# EQMap.NET - EverQuest Mapping and Protocol Analysis Suite

A comprehensive toolkit for EverQuest server development, featuring real-time mapping, packet analysis, and protocol debugging tools.

## 🎮 What is EQMap.NET?

EQMap.NET is a suite of tools designed for EverQuest private server development and protocol analysis. It provides real-time visualization of game data, comprehensive packet analysis, and debugging capabilities for EQEmu server development.

## 🎥 Demo Videos

- **[EQMap Real-time Mapping Demo](https://www.youtube.com/watch?v=eosSt0Vvpjk)** - See the real-time mapping functionality in action
- **[BOT appears in Client](https://www.youtube.com/watch?v=-uP2zkLXSPw)** - See a LUA bot appear in the client

## 🚀 Key Features

### 🗺️ Real-time Mapping (EQMap)
- Live visualization of player positions, NPCs, and game objects
- Zone mapping with dynamic updates
- Multi-player tracking and session management
- Lua scripting support for custom behaviors

### 📊 Packet Analysis (EQLogs)
- Comprehensive packet capture and analysis
- Support for server and bot log comparison
- Session-based filtering and organization
- Real-time packet structure parsing using reflection
- Bidirectional packet support (ToServer/FromServer)
- Hex dump visualization with structure breakdown

### 🔧 Protocol Library (EQProtocol)
- Complete EverQuest protocol implementation
- Automatic opcode-to-structure mapping
- Support for compression (Zlib) and encryption (DES)
- Extensive packet structure definitions
- Cross-platform .NET Standard 2.1 compatibility

## 📁 Project Structure

```
eqmap.net/
├── EQMap/              # Real-time mapping application
├── EQLogs/             # Packet analysis and logging tools
├── EQProtocol/         # EverQuest protocol library
├── EQConsole/          # Console utilities
└── docs/               # Documentation
```

## 📖 Documentation Index

### Getting Started
- [Installation Guide](docs/INSTALLATION.md) *(coming soon)*
- [Quick Start Tutorial](docs/QUICKSTART.md) *(coming soon)*
- [Configuration Guide](docs/CONFIGURATION.md) *(coming soon)*

### EQMap (Real-time Mapping)
- [EQMap User Guide](docs/eqmap/USER_GUIDE.md) *(coming soon)*
- [Lua Scripting Reference](docs/eqmap/LUA_SCRIPTING.md) *(coming soon)*
- [Zone Data Format](docs/eqmap/ZONE_FORMAT.md) *(coming soon)*

### EQLogs (Packet Analysis)
- [Packet Analysis Guide](docs/eqlogs/PACKET_ANALYSIS.md) *(coming soon)*
- [Session Filtering](docs/eqlogs/SESSION_FILTERING.md) *(coming soon)*
- [Log File Formats](docs/eqlogs/LOG_FORMATS.md) *(coming soon)*
- [Large File Handling](LARGE_LOG_FILE_SOLUTION.md)

### EQProtocol (Protocol Library)
- [Protocol Reference](docs/eqprotocol/PROTOCOL_REFERENCE.md) *(coming soon)*
- [Packet Structures](docs/eqprotocol/PACKET_STRUCTURES.md) *(coming soon)*
- [Encryption and Compression](docs/eqprotocol/ENCRYPTION.md) *(coming soon)*

### Development
- [Contributing Guidelines](docs/CONTRIBUTING.md) *(coming soon)*
- [Building from Source](docs/BUILD_GUIDE.md) *(coming soon)*
- [API Documentation](docs/API_REFERENCE.md) *(coming soon)*

## 🔧 Technical Highlights

### Advanced Packet Analysis
- **Reflection-based Structure Mapping**: Automatically maps opcodes to packet structures using naming conventions
- **Bidirectional Packet Support**: Handles ToServer/FromServer packet variants seamlessly
- **Session Management**: Filter and organize packets by game sessions with automatic session detection
- **Multiple Log Sources**: Compare server logs with bot logs for comprehensive analysis

### Real-time Capabilities
- **Live Data Streaming**: Real-time updates from running EQEmu servers
- **Multi-session Support**: Track multiple players and sessions simultaneously
- **Performance Optimized**: Efficient handling of large data streams and log files

### Protocol Implementation
- **Complete Coverage**: Extensive implementation of EverQuest network protocol
- **Modern Architecture**: Clean .NET Standard 2.1 implementation with async/await patterns
- **Extensible Design**: Easy to add new packet types and extend functionality

## 🛠️ Built With

- **.NET 8.0** - Modern cross-platform framework
- **WPF** - Rich desktop UI for Windows applications
- **MVVM Pattern** - Clean separation of concerns
- **NLog** - Comprehensive logging framework
- **Docker Integration** - Seamless integration with containerized EQEmu setups

## 🤝 Contributing

This project is actively developed for EQEmu server development and protocol research. Contributions are welcome!

## 📄 License

This project is developed for educational and private server development purposes. Please respect the intellectual property rights of the original EverQuest game.

## 🙋 Support

For questions, issues, or contributions:
- Open an issue on this repository
- Check the documentation links above
- Review the demo videos for usage examples

---

*EQMap.NET - Bringing modern development tools to classic EverQuest server development*