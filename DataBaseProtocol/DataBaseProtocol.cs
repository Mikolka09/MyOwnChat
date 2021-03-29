using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using ProtocolsMessages;


namespace DataBaseProtocol
{
    public interface Datas { }

    public class DataList : Datas
    {
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
        public List<Contact> Contacts { get; set; }

    }

    public class SaveData
    {
        public static DataBase dataBase = new DataBase(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
        public static void SaveUser(User user)
        {
            dataBase.Users.InsertOnSubmit(
                new UserB
                {
                    Login = user.Login,
                    Password = user.Password,
                    CountBadWord = user.CountBadWord,
                    Birthday = user.Birthday
                });
            dataBase.SubmitChanges();
        }

        public static void SaveMessage(Message message)
        {
            var queryS = dataBase.Users.Where(c => c.Login == message.LoginSend).Select((d) => d.Id);

            var queryR = dataBase.Users.Where(c => c.Login == message.LoginReceive).Select((d) => d.Id);


            dataBase.Messages.InsertOnSubmit(
                new MessageB
                {
                    Text = message.Text,
                    Priorety = message.Priorety,
                    IdSend = Convert.ToInt32(queryS.ToString()),
                    IdReceive = Convert.ToInt32(queryR.ToString()),
                    Answer = message.Answer,
                    Moment = Convert.ToDateTime(message.Moment)
                });
            dataBase.SubmitChanges();
        }

        public static void SaveContact(Contact contact)
        {
            var query = dataBase.Users.Where(c => c.Login == contact.Login).Select((d) => d.Id);

            dataBase.Contacts.InsertOnSubmit(
                new ContactB
                {
                    IdLogin = Convert.ToInt32(query.ToString()),
                    Name = contact.Name,
                    Tag = contact.Tag,
                    Color = contact.Color
                });
            dataBase.SubmitChanges();
        }
    }

    public class LoadData
    {
        public static DataBase dataBase = new DataBase(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);

        public static Datas LoadUser()
        {
            DataList data = new DataList();
            data.Users = new List<User>();

            var query = from U in dataBase.Users
                        select U;
            foreach (var item in query)
            {
                User user = new User();
                user.Login = item.Login;
                user.Password = item.Password;
                user.CountBadWord = item.CountBadWord;
                user.Birthday = item.Birthday;
                data.Users.Add(user);
            }
            return data;
        }

        public static Datas LoadMessage()
        {
            DataList data = new DataList();
            data.Messages = new List<Message>();

            var query = from M in dataBase.Messages
                        join US in dataBase.Users on M.IdSend equals US.Id
                        join UR in dataBase.Users on M.IdReceive equals UR.Id
                        select new MixedMess { Text = M.Text, Priorety = M.Priorety, LoginSend = US.Login, LoginReceive = UR.Login,
                        Answer = M.Answer, Moment = M.Moment.ToLongTimeString()};
            foreach (var item in query)
            {
                Message mess = new Message();
                mess.Text = item.Text;
                mess.Priorety = item.Priorety;
                mess.LoginSend = item.LoginSend;
                mess.LoginReceive = item.LoginReceive;
                mess.Answer = item.Answer;
                mess.Moment = item.Moment;
                data.Messages.Add(mess);
            }
            return data;
        }

        public static Datas LoadContact()
        {
            DataList data = new DataList();
            data.Contacts = new List<Contact>();

            var query = from C in dataBase.Contacts
                        join U in dataBase.Users on C.IdLogin equals U.Id
                        select new { U.Login, C.Name, C.Tag, C.Color};
            foreach (var item in query)
            {
                Contact contact = new Contact();
                contact.Login = item.Login;
                contact.Name = item.Name;
                contact.Tag = item.Tag;
                contact.Color = item.Color;
                data.Contacts.Add(contact);
            }
            return data;
        }

    }

    public class MixedMess
    {
       
        public string Text { get; set; }

        public string Priorety { get; set; }

        public string LoginSend { get; set; }

        public string LoginReceive { get; set; }

        public string Answer { get; set; }

        public string Moment { get; set; }
    }

    [Table(Name = "User")]
    public class UserB
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "login")]
        public string Login { get; set; }
        [Column(Name = "password")]
        public string Password { get; set; }
        [Column(Name = "count_bad_word")]
        public int CountBadWord { get; set; }
        [Column(Name = "birthday")]
        public string Birthday { get; set; }

        public TcpClient ClientTcp { get; set; }

        public IPEndPoint EndPointClient { get; set; }
    }

    [Table(Name = "Message")]
    public class MessageB
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "text")]
        public string Text { get; set; }
        [Column(Name = "priorety")]
        public string Priorety { get; set; }
        [Column(Name = "id_send")]
        public int IdSend { get; set; }
        [Column(Name = "id_receive")]
        public int IdReceive { get; set; }
        [Column(Name = "answer")]
        public string Answer { get; set; }
        [Column(Name = "moment")]
        public DateTime Moment { get; set; }

    }

    [Table(Name = "Contact")]
    public class ContactB
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "id_login")]
        public int IdLogin { get; set; }
        [Column(Name = "name")]
        public string Name { get; set; }
        [Column(Name = "tag")]
        public string Tag { get; set; }
        [Column(Name = "color")]
        public Color Color { get; set; }
    }

    public class DataBase : DataContext
    {
        public Table<UserB> Users;
        public Table<MessageB> Messages;
        public Table<ContactB> Contacts;

        public DataBase(string conStr) : base(conStr)
        {
            Users = GetTable<UserB>();
            Messages = GetTable<MessageB>();
            Contacts = GetTable<ContactB>();

        }

    }
}
