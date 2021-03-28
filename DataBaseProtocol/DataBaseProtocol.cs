using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Sockets;
using System.Net;

namespace DataBaseProtocol
{
    public class DataBaseProtocol
    {

    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int CountBadWord { get; set; }
        public string Birthday { get; set; }
        public TcpClient ClientTcp { get; set; }
        public IPEndPoint EndPointClient { get; set; }
    }

    public class Message
    {
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string Priorety { get; set; }
        public int IdSend { get; set; }
        public int IdReceive { get; set; }
        public string Answer { get; set; }

    }
    public class Messages
    {
        public int IdLogin { get; set; }

    }
}
