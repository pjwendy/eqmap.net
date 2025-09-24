using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenEQ.Netcode.GameClient.Events
{
    public class FilePacketEventEmitter : IPacketEventEmitter, IDisposable
    {
        private readonly string _baseDirectory;
        private readonly string _sessionId;
        private StreamWriter _eventWriter;
        private StreamWriter _packetWriter;
        private readonly object _writeLock = new object();
        private readonly Timer _flushTimer;
        private readonly JsonSerializerSettings _jsonSettings;

        public RecordingMode RecordingMode { get; set; }

        public FilePacketEventEmitter(string baseDirectory = null, RecordingMode recordingMode = RecordingMode.Full)
        {
            RecordingMode = recordingMode;
            _sessionId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            _baseDirectory = baseDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), "packet_captures");
            
            _jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new StringEnumConverter(), new RawPacketJsonConverter() }
            };

            if (RecordingMode != RecordingMode.Off)
            {
                InitializeWriters();
            }

            // Auto-flush every 5 seconds
            _flushTimer = new Timer(_ => Flush(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        }

        private void InitializeWriters()
        {
            Directory.CreateDirectory(_baseDirectory);
            var sessionDir = Path.Combine(_baseDirectory, _sessionId);
            Directory.CreateDirectory(sessionDir);

            if (RecordingMode == RecordingMode.EventsOnly || RecordingMode == RecordingMode.Full || RecordingMode == RecordingMode.Replay)
            {
                var eventFile = Path.Combine(sessionDir, "events.jsonl");
                _eventWriter = new StreamWriter(eventFile, append: true, encoding: Encoding.UTF8);
                WriteMetadata(_eventWriter, "event_log");
            }

            if (RecordingMode == RecordingMode.PacketsOnly || RecordingMode == RecordingMode.Full || RecordingMode == RecordingMode.Replay)
            {
                var packetFile = Path.Combine(sessionDir, "packets.jsonl");
                _packetWriter = new StreamWriter(packetFile, append: true, encoding: Encoding.UTF8);
                WriteMetadata(_packetWriter, "packet_log");
            }
        }

        private void WriteMetadata(StreamWriter writer, string logType)
        {
            var metadata = new
            {
                type = "metadata",
                log_type = logType,
                session_id = _sessionId,
                start_time = DateTime.UtcNow,
                recording_mode = RecordingMode.ToString()
            };
            
            writer.WriteLine(JsonConvert.SerializeObject(metadata, _jsonSettings));
            writer.Flush();
        }

        public void EmitRawPacket(PacketDirection direction, byte[] data, string packetType, string streamType)
        {
            if (RecordingMode == RecordingMode.Off || RecordingMode == RecordingMode.EventsOnly)
                return;

            var packet = new RawPacket
            {
                Direction = direction,
                StreamType = streamType,
                PacketType = packetType,
                Data = data,
                Size = data?.Length ?? 0
            };

            lock (_writeLock)
            {
                _packetWriter?.WriteLine(JsonConvert.SerializeObject(packet, _jsonSettings));
            }
        }

        public void EmitGameEvent(GameEvent gameEvent)
        {
            if (RecordingMode == RecordingMode.Off || RecordingMode == RecordingMode.PacketsOnly)
                return;

            lock (_writeLock)
            {
                _eventWriter?.WriteLine(JsonConvert.SerializeObject(gameEvent, _jsonSettings));
            }
        }

        public void Flush()
        {
            lock (_writeLock)
            {
                _eventWriter?.Flush();
                _packetWriter?.Flush();
            }
        }

        public void Dispose()
        {
            _flushTimer?.Dispose();
            Flush();
            
            lock (_writeLock)
            {
                if (_eventWriter != null)
                {
                    WriteMetadata(_eventWriter, "event_log_end");
                    _eventWriter.Dispose();
                }
                
                if (_packetWriter != null)
                {
                    WriteMetadata(_packetWriter, "packet_log_end");
                    _packetWriter.Dispose();
                }
            }
        }
    }

    public class RawPacketJsonConverter : JsonConverter<RawPacket>
    {
        public override void WriteJson(JsonWriter writer, RawPacket value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("Timestamp");
            serializer.Serialize(writer, value.Timestamp);
            
            writer.WritePropertyName("Direction");
            serializer.Serialize(writer, value.Direction);
            
            writer.WritePropertyName("StreamType");
            writer.WriteValue(value.StreamType);
            
            writer.WritePropertyName("PacketType");
            writer.WriteValue(value.PacketType);
            
            writer.WritePropertyName("DataHex");
            writer.WriteValue(value.DataHex);
            
            writer.WritePropertyName("Size");
            writer.WriteValue(value.Size);
            
            if (value.Sequence.HasValue)
            {
                writer.WritePropertyName("Sequence");
                writer.WriteValue(value.Sequence.Value);
            }
            
            writer.WriteEndObject();
        }

        public override RawPacket ReadJson(JsonReader reader, Type objectType, RawPacket existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Reading RawPacket from JSON is not supported");
        }
    }
}