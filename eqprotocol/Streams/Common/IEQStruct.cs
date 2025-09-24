using System.IO;

namespace EQProtocol.Streams.Common {
    public interface IEQStruct {
        void Unpack(byte[] data, int offset = 0);
        void Unpack(BinaryReader br);
        byte[] Pack();
        void Pack(BinaryWriter bw);
    }
}
