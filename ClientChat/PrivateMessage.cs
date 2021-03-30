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
    public partial class PrivateMessage : Form
    {
        public string message { get; set; }

        public PrivateMessage()
        {
            InitializeComponent();
        }

        private void PrivateMessage_Load(object sender, EventArgs e)
        {
            message = "";
            string log = (Owner as Form1).message.LoginReceive;
            this.Text = $"PRIVATE MESSAGE FROM: {log}";
            textBoxMessage.Clear();
            buttonSend.Enabled = false;
        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {
            buttonSend.Enabled = true;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            message = textBoxMessage.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
