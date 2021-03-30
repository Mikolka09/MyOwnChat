using ProtocolsMessages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientChat
{
    public partial class CreateGroup : Form
    {
        public List<Contact> contacts { get; set; } = new List<Contact>();
        public List<string> contactsCheck { get; set; } = new List<string>();

        public string nameGroup { get; set; }


        public CreateGroup()
        {
            InitializeComponent();
        }

        private void CreateGroup_Load(object sender, EventArgs e)
        {
            buttonCreateGroup.Enabled = false;
            contacts = (Owner as Form1).contacts;
            foreach (var item in contacts)
            {
                ListViewItem it = new ListViewItem(item.Login);
                listViewContacts.Items.Add(it);
            }
        }

        private void textBoxNameGroup_TextChanged(object sender, EventArgs e)
        {
            buttonCreateGroup.Enabled = true;
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
            {
                foreach (ListViewItem item in listViewContacts.Items)
                {
                        item.Checked = true;
                }
            }
            else
            {
                foreach (ListViewItem item in listViewContacts.Items)
                {
                    item.Checked = false;
                }
            }
        }

        private void buttonCreateGroup_Click(object sender, EventArgs e)
        {
            nameGroup = textBoxNameGroup.Text;
            foreach (ListViewItem item in listViewContacts.Items)
            {
                if (item.Checked)
                {
                    contactsCheck.Add(item.Text);
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
