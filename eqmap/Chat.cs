using OpenEQ.Netcode;
using OpenEQ.Netcode.GameClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eqmap
{
    public class Chat
    {
        EQGameClient gameClient;
        Account account;

        public Chat(Account account, EQGameClient gameClient) 
        { 
            this.account = account;  
            this.gameClient = gameClient; 
        }
        
        public void Say(string message)
        {
            gameClient?.SendChat(message, ChatChannel.Say);
        }
        
        public void Group(string message)
        {
            gameClient?.SendChat(message, ChatChannel.Group);
        }
        
        public void Guild(string message)
        {
            gameClient?.SendChat(message, ChatChannel.Guild);
        }
        
        public void Tell(string target, string message)
        {
            // EQGameClient doesn't have SendTell yet, use Say with /tell format
            gameClient?.SendChat($"/tell {target} {message}", ChatChannel.Say);
        }
        
        public void Ooc(string message)
        {
            gameClient?.SendChat(message, ChatChannel.OOC);
        }
        
        public void Shout(string message)
        {
            gameClient?.SendChat(message, ChatChannel.Shout);
        }
    }
}
