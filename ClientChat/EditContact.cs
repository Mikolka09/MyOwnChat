using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ClientChat
{
    public partial class EditContact : Form
    {
        public Contact contact { get; set; }
        public EditContact()
        {
            InitializeComponent();
        }

        private void EditContact_Load(object sender, EventArgs e)
        {
            contact = (Owner as Form1).Cont;
            textBoxLogin.Text = contact.Login;
            textBoxName.Text = contact.Name;
            textBoxTag.Text = contact.Tag;
            textBoxLogin.Enabled = false;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Regex regN = new Regex("^[A-ZА-Я]{1}[a-zа-я]{1,10}$");
            if (!regN.IsMatch(textBoxName.Text))
            {
                MessageBox.Show("Name entered incorrectly", "Warning",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
                contact.Name = textBoxName.Text;
            contact.Tag = textBoxTag.Text;
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

}
