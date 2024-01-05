using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eqmap
{
    public class Chat
    {
        ZoneStream zoneStream;
        Account account;

        public Chat(Account account, ZoneStream zoneStream) { this.account = account;  this.zoneStream = zoneStream; }
        public void Say(string message)
        {
            zoneStream.SendChatMessage(string.Empty, string.Empty, message);
        }
        public void Group(string message)
        {

        }
        public void Guild(string message)
        {

        }
        public void Tell(string target, string message)
        {
            zoneStream.SendChatMessage(target, account.Character, message);
        }
        public void Ooc(string message)
        {

        }
        public void Shout(string message)
        {

        }
    }
}
