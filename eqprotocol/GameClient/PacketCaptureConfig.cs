using OpenEQ.Netcode.GameClient.Events;

namespace OpenEQ.Netcode.GameClient
{
    public class PacketCaptureConfig
    {
        public RecordingMode Mode { get; set; } = RecordingMode.Off;
        public string OutputDirectory { get; set; } = "";
        public bool IncludePacketHexDumps { get; set; } = false;
        public bool IncludeSemanticEvents { get; set; } = true;
        
        public static PacketCaptureConfig CreateForNarrationEngine(string outputDir = "")
        {
            return new PacketCaptureConfig
            {
                Mode = RecordingMode.Full,
                OutputDirectory = outputDir,
                IncludePacketHexDumps = false,
                IncludeSemanticEvents = true
            };
        }
        
        public static PacketCaptureConfig CreateForDebugging(string outputDir = "")
        {
            return new PacketCaptureConfig
            {
                Mode = RecordingMode.Full,
                OutputDirectory = outputDir,
                IncludePacketHexDumps = true,
                IncludeSemanticEvents = false
            };
        }
        
        public static PacketCaptureConfig Disabled => new PacketCaptureConfig
        {
            Mode = RecordingMode.Off
        };
    }
}