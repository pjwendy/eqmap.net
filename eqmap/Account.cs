using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eqmap
{
    public class Account
    {
        public delegate void LogonEventHandler();
        public event LogonEventHandler OnLogon;

        public string LoginServer;
        public int LoginServerPort;
        public string User;
        public string Password;
        public string Server;
        public string Character;
        public void Logon(string loginserver, int loginserverport, string user, string password, string server, string character)
        {
            this.LoginServer = loginserver;
            this.LoginServerPort = loginserverport;
            this.User = user;
            this.Password = password;
            this.Server = server;
            this.Character = character;
            OnLogon?.Invoke();            
        }
    }
}
