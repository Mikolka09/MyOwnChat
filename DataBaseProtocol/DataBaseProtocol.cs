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
using System.Data.Entity;

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
            try
            {
                dataBase.Users.InsertOnSubmit(
                                new UserB
                                {
                                    Login = user.Login,
                                    Password = user.Password,
                                    Tag = user.Tag,
                                    CountBadWord = user.CountBadWord,
                                    Birthday = user.Birthday,
                                    IPClient = user.IPClient
                                });
                dataBase.SubmitChanges();
            }
            catch { }

        }


        public static void SaveUsers(List<User> users)
        {
            try
            {
                foreach (var item in users)
                {
                    UserB user = new UserB();

                    user.Login = item.Login;
                    user.Password = item.Password;
                    user.Tag = item.Tag;
                    user.CountBadWord = item.CountBadWord;
                    user.Birthday = item.Birthday;
                    user.IPClient = item.IPClient;

                    dataBase.Users.InsertOnSubmit(user);
                    dataBase.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void SaveListUser(List<User> users)
        {
            var list = from U in dataBase.Users
                       select U;
            if (list.ToList().Count != 0)
            {
                dataBase.Users.DeleteAllOnSubmit(list);
                dataBase.ExecuteCommand(@"DELETE FROM [User]
                                          DBCC CHECKIDENT('User', RESEED, 0)");
                dataBase.SubmitChanges();
                SaveUsers(users);
            }
            else
            {
                SaveUsers(users);
            }
        }

        public static void SaveMessage(Message message)
        {
            var queryS = dataBase.Users.Where(c => c.Login == message.LoginSend).Select((d) => d.Id).ToList();

            var queryR = dataBase.Users.Where(c => c.Login == message.LoginReceive).Select((d) => d.Id).ToList();
            int id = 0;
            if (queryR.Count == 0)
                id = 0;
            else
                id = queryR[0];

            try
            {
                dataBase.Messages.InsertOnSubmit(
                                new MessageB
                                {
                                    Text = message.Text,
                                    Priorety = message.Priorety,
                                    IdSend = queryS[0],
                                    IdReceive = id,
                                    Answer = message.Answer,
                                    Moment = Convert.ToDateTime(message.Moment)
                                });
                dataBase.SubmitChanges();
            }
            catch { }
        }

        public static void SaveListContacts(List<Contact> contacts)
        {
            try
            {
                foreach (var item in contacts)
                {
                    var queryL = dataBase.Users.Where(c => c.Login == item.Login).Select((d) => d.Id).ToList();
                    var queryA = dataBase.Users.Where(c => c.Login == item.LoginAdd).Select((d) => d.Id).ToList();


                    ContactB cont = new ContactB();

                    cont.IdLogin = queryL[0];
                    cont.IdLoginAdd = queryA[0];
                    cont.Name = item.Name;
                    cont.Tag = item.Tag;
                    cont.Color = item.Color.ToString();
                    dataBase.Contacts.InsertOnSubmit(cont);
                }
                dataBase.SubmitChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



        public static void SaveContacts(List<Contact> contacts)
        {
            var list = from C in dataBase.Contacts
                       select C;
            if (list.ToList().Count == 0)
            {
                SaveListContacts(contacts);
            }
            else
            {
                dataBase.Contacts.DeleteAllOnSubmit(list);
                dataBase.ExecuteCommand(@"DELETE FROM [Contact]
                                          DBCC CHECKIDENT('Contact', RESEED, 0)");
                dataBase.SubmitChanges();
                SaveListContacts(contacts);
            }

        }

        public static void SaveContact(List<Contact> contacts)
        {
            var cont = from C in dataBase.Contacts
                       select C;
            if (cont.ToList().Count != 0)
            {
                dataBase.Contacts.DeleteAllOnSubmit(cont);
                dataBase.SubmitChanges();

                SaveContacts(contacts);
            }
            else
            {
                SaveContacts(contacts);
            }

        }
    }

    public class LoadData
    {
        public static DataBase dataBase = new DataBase(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);

        public static List<User> LoadUser()
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
                user.Tag = item.Tag;
                user.CountBadWord = item.CountBadWord;
                user.Birthday = item.Birthday;
                user.IPClient = item.IPClient;
                data.Users.Add(user);
            }
            return data.Users;
        }

        public static List<Message> LoadMessage()
        {
            DataList data = new DataList();
            data.Messages = new List<Message>();

            var query = from M in dataBase.Messages
                        join US in dataBase.Users on M.IdSend equals US.Id into outer1
                        join UR in dataBase.Users on M.IdReceive equals UR.Id into outer2
                        from item1 in outer1.DefaultIfEmpty()
                        from item2 in outer2.DefaultIfEmpty()
                        select new MixedMess
                        {
                            Text = M.Text,
                            Priorety = M.Priorety,
                            LoginSend = item1.Login == null ? "0" : item1.Login,
                            LoginReceive = item2 == null ? "0" : item2.Login,
                            Answer = M.Answer,
                            Moment = M.Moment.ToLongTimeString()
                        };
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
            return data.Messages;
        }

        public static List<Contact> LoadContact()
        {
            DataList data = new DataList();
            data.Contacts = new List<Contact>();

            var query = from C in dataBase.Contacts
                        join U1 in dataBase.Users on C.IdLogin equals U1.Id
                        join U2 in dataBase.Users on C.IdLoginAdd equals U2.Id
                        select new MixedCont { Login = U1.Login, LoginAdd = U2.Login, Name = C.Name, Tag = C.Tag, Color = C.Color };
            foreach (var item in query)
            {
                Contact contact = new Contact();
                contact.Login = item.Login;
                contact.LoginAdd = item.LoginAdd;
                contact.Name = item.Name;
                contact.Tag = item.Tag;
                contact.Color = Color.FromName(item.Color);
                data.Contacts.Add(contact);
            }
            return data.Contacts;
        }

    }

    public class MixedCont
    {
        public string Login { get; set; }

        public string LoginAdd { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public string Color { get; set; }
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
        [Column(Name = "tag")]
        public string Tag { get; set; }
        [Column(Name = "count_bad_word")]
        public int CountBadWord { get; set; }
        [Column(Name = "birthday")]
        public string Birthday { get; set; }
        [Column(Name = "ip_client")]
        public string IPClient;

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
        [Column(Name = "id_loginAdd")]
        public int IdLoginAdd { get; set; }
        [Column(Name = "name")]
        public string Name { get; set; }
        [Column(Name = "tag")]
        public string Tag { get; set; }
        [Column(Name = "color")]
        public string Color { get; set; }
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
