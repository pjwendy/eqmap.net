using System;
using OpenEQ.Netcode;

namespace EQBot.GameClient.Models
{
    public class ChatMessage
    {
        public ChatChannel Channel { get; set; }
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public string Message { get; set; } = "";
        public uint Language { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        public bool IsFromGM { get; set; }
        public bool IsSystem => string.IsNullOrEmpty(From);
        public bool IsTell => Channel == ChatChannel.Tell;
        public bool IsGuild => Channel == ChatChannel.Guild;
        public bool IsGroup => Channel == ChatChannel.Group;
        public bool IsSay => Channel == ChatChannel.Say;
        public bool IsShout => Channel == ChatChannel.Shout;
        public bool IsOOC => Channel == ChatChannel.OOC;
        public bool IsAuction => Channel == ChatChannel.Auction;
        
        public override string ToString()
        {
            if (IsSystem)
                return $"[SYSTEM] {Message}";
                
            var channelName = Channel.ToString().ToUpper();
            
            if (IsTell)
                return $"[{channelName}] {From} -> {To}: {Message}";
            else if (IsGuild && !string.IsNullOrEmpty(From))
                return $"[{channelName}] {From}: {Message}";
            else
                return $"[{channelName}] {From}: {Message}";
        }
    }
}