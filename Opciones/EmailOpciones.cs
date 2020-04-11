using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tateti.Opciones
{
    public class EmailOpciones
    {
        public string MailType { get; set; }
        public string MailServer { get; set; }
        public string MailPort { get; set; }
        public string UseSSL { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string RemoteServerAPI { get; set; }
        public string RemoteServerKey { get; set; }

        public EmailOpciones() { }

        public EmailOpciones(string mailType, string mailServer, 
            string mailPort, string useSSL, string userId, 
            string password, string remoteServerAPI, string remoteServerKey)
        {
            MailType = mailType;
            MailServer = mailServer;
            MailPort = mailPort;
            UseSSL = useSSL;
            UserId = 
            Password = password;
            RemoteServerAPI = remoteServerAPI;
            RemoteServerKey = remoteServerKey;
        }
    }
}
