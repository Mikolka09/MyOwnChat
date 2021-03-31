using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing;


namespace ProtocolsMessages
{
    public interface Data { }


    [Serializable]
    public class DataMessage : Data
    {
        public Message Message { get; set; }
    }

    [Serializable]
    public class DataContact : Data
    {
        public Contact Contact { get; set; }
    }

    [Serializable]
    public class DataUser : Data
    {
        public User User { get; set; }
    }

    [Serializable]
    public class DataUsers : Data
    {
        public List<User> ListU { get; set; }
    }

    [Serializable]
    public class DataContacts : Data
    {
        public List<Contact> ListC { get; set; }
    }

    [Serializable]
    public class DataFile : Data
    {
        public List<byte[]> FileByte { get; set; }
    }

    public class Transfer
    {
        private static BinaryFormatter formatter = new BinaryFormatter();

        public static void SendTCP(TcpClient client, Data data)
        {
            try
            {
                formatter.Serialize(client.GetStream(), data);
            }
            catch { }

        }

        public static Data ReceiveTCP(TcpClient client)
        {
            try
            {
                return (Data)formatter.Deserialize(client.GetStream());
            }
            catch (IOException) { throw; }


        }
    }


    [Serializable]
    public class User
    {

        public string Login { get; set; }

        public string Password { get; set; }

        public string Tag { get; set; }

        public int CountBadWord { get; set; }

        public string Birthday { get; set; }
        public string IPClient { get; set; }
        [NonSerialized]
        public TcpClient ClientTcp;
        [NonSerialized]
        public IPEndPoint EndPointClient;
    }

    [Serializable]
    public class Message
    {

        public string Text { get; set; }

        public string Priorety { get; set; }

        public string LoginSend { get; set; }

        public string LoginReceive { get; set; }

        public string Answer { get; set; }

        public string Moment { get; set; }

    }

    [Serializable]
    public class Contact
    {
        public string Login { get; set; }

        public string LoginAdd { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public Color Color { get; set; }
    }



}
