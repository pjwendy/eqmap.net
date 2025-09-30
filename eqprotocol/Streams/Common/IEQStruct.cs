using System.IO;

namespace EQProtocol.Streams.Common {
    /// <summary>
    /// Core interface for all EverQuest packet structures that can be serialized to/from binary data.
    /// Provides methods for packing and unpacking data structures used in EverQuest network communication.
    /// </summary>
    public interface IEQStruct {
        /// <summary>
        /// Unpacks binary data into the structure starting at the specified offset.
        /// </summary>
        /// <param name="data">The binary data array containing the packed structure</param>
        /// <param name="offset">The byte offset within the data array to start unpacking from (default: 0)</param>
        void Unpack(byte[] data, int offset = 0);

        /// <summary>
        /// Unpacks binary data into the structure using a BinaryReader.
        /// This method provides more advanced reading capabilities and proper endianness handling.
        /// </summary>
        /// <param name="br">The BinaryReader positioned at the start of the structure data</param>
        void Unpack(BinaryReader br);

        /// <summary>
        /// Packs the structure into a binary byte array suitable for network transmission.
        /// </summary>
        /// <returns>A byte array containing the packed structure data</returns>
        byte[] Pack();

        /// <summary>
        /// Packs the structure into binary format using a BinaryWriter.
        /// This method provides more advanced writing capabilities and proper endianness handling.
        /// </summary>
        /// <param name="bw">The BinaryWriter to write the structure data to</param>
        void Pack(BinaryWriter bw);
    }
}
