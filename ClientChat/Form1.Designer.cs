
namespace ClientChat
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.listViewMessages = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripMessages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendPrivateMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripContacts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listViewClients = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxGroups = new System.Windows.Forms.CheckBox();
            this.buttonLoadContacts = new System.Windows.Forms.Button();
            this.buttonSaveContacts = new System.Windows.Forms.Button();
            this.listViewContacts = new System.Windows.Forms.ListView();
            this.columnLogin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSendFileAll = new System.Windows.Forms.Button();
            this.visualStyler1 = new SkinSoft.VisualStyler.VisualStyler(this.components);
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.contextMenuStripMessages.SuspendLayout();
            this.contextMenuStripContacts.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visualStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.SystemColors.Control;
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExit.Location = new System.Drawing.Point(697, 508);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(91, 36);
            this.buttonExit.TabIndex = 3;
            this.buttonExit.Text = "EXIT";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(140, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "WINDOW MESSAGES CHAT";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxMessage.Location = new System.Drawing.Point(12, 442);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(427, 34);
            this.textBoxMessage.TabIndex = 5;
            this.textBoxMessage.TextChanged += new System.EventHandler(this.textBoxMessage_TextChanged);
            // 
            // buttonSend
            // 
            this.buttonSend.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSend.Location = new System.Drawing.Point(445, 442);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(91, 36);
            this.buttonSend.TabIndex = 7;
            this.buttonSend.Text = "SEND";
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // listViewMessages
            // 
            this.listViewMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.listViewMessages.ContextMenuStrip = this.contextMenuStripMessages;
            this.listViewMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewMessages.Location = new System.Drawing.Point(12, 32);
            this.listViewMessages.Name = "listViewMessages";
            this.listViewMessages.Size = new System.Drawing.Size(524, 404);
            this.listViewMessages.TabIndex = 9;
            this.listViewMessages.UseCompatibleStateImageBehavior = false;
            this.listViewMessages.View = System.Windows.Forms.View.Details;
            this.listViewMessages.DoubleClick += new System.EventHandler(this.toolStripMenuItemAdd_Click);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "SERVER";
            this.columnHeader4.Width = 518;
            // 
            // contextMenuStripMessages
            // 
            this.contextMenuStripMessages.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMessages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendPrivateMessageToolStripMenuItem,
            this.sendFileToolStripMenuItem});
            this.contextMenuStripMessages.Name = "contextMenuStripMessages";
            this.contextMenuStripMessages.Size = new System.Drawing.Size(223, 52);
            // 
            // sendPrivateMessageToolStripMenuItem
            // 
            this.sendPrivateMessageToolStripMenuItem.Name = "sendPrivateMessageToolStripMenuItem";
            this.sendPrivateMessageToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.sendPrivateMessageToolStripMenuItem.Text = "Send Private Message";
            this.sendPrivateMessageToolStripMenuItem.Click += new System.EventHandler(this.sendPrivateMessageToolStripMenuItem_Click);
            // 
            // sendFileToolStripMenuItem
            // 
            this.sendFileToolStripMenuItem.Name = "sendFileToolStripMenuItem";
            this.sendFileToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.sendFileToolStripMenuItem.Text = "Send File Private";
            this.sendFileToolStripMenuItem.Click += new System.EventHandler(this.sendFileToolStripMenuItem_Click);
            // 
            // contextMenuStripContacts
            // 
            this.contextMenuStripContacts.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripContacts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.addToolStripMenuItem});
            this.contextMenuStripContacts.Name = "contextMenuStrip1";
            this.contextMenuStripContacts.Size = new System.Drawing.Size(123, 76);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listViewClients);
            this.groupBox2.Controls.Add(this.checkBoxGroups);
            this.groupBox2.Controls.Add(this.buttonLoadContacts);
            this.groupBox2.Controls.Add(this.buttonSaveContacts);
            this.groupBox2.Controls.Add(this.listViewContacts);
            this.groupBox2.Location = new System.Drawing.Point(542, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(246, 465);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MY CONTACTS";
            // 
            // listViewClients
            // 
            this.listViewClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewClients.ContextMenuStrip = this.contextMenuStripContacts;
            this.listViewClients.Location = new System.Drawing.Point(6, 22);
            this.listViewClients.Name = "listViewClients";
            this.listViewClients.Size = new System.Drawing.Size(234, 403);
            this.listViewClients.TabIndex = 16;
            this.listViewClients.UseCompatibleStateImageBehavior = false;
            this.listViewClients.View = System.Windows.Forms.View.Details;
            this.listViewClients.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewContacts_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "LOGIN";
            this.columnHeader1.Width = 75;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "CountB";
            this.columnHeader2.Width = 72;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "BIRTHDAY";
            this.columnHeader3.Width = 81;
            // 
            // checkBoxGroups
            // 
            this.checkBoxGroups.AutoSize = true;
            this.checkBoxGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxGroups.Location = new System.Drawing.Point(148, 434);
            this.checkBoxGroups.Name = "checkBoxGroups";
            this.checkBoxGroups.Size = new System.Drawing.Size(83, 21);
            this.checkBoxGroups.TabIndex = 15;
            this.checkBoxGroups.Text = "Groups";
            this.checkBoxGroups.UseVisualStyleBackColor = true;
            this.checkBoxGroups.CheckedChanged += new System.EventHandler(this.checkBoxGroups_CheckedChanged);
            // 
            // buttonLoadContacts
            // 
            this.buttonLoadContacts.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLoadContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLoadContacts.Location = new System.Drawing.Point(73, 428);
            this.buttonLoadContacts.Name = "buttonLoadContacts";
            this.buttonLoadContacts.Size = new System.Drawing.Size(65, 32);
            this.buttonLoadContacts.TabIndex = 14;
            this.buttonLoadContacts.Text = "Load";
            this.buttonLoadContacts.UseVisualStyleBackColor = false;
            this.buttonLoadContacts.Click += new System.EventHandler(this.buttonLoadContacts_Click);
            // 
            // buttonSaveContacts
            // 
            this.buttonSaveContacts.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSaveContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSaveContacts.Location = new System.Drawing.Point(6, 427);
            this.buttonSaveContacts.Name = "buttonSaveContacts";
            this.buttonSaveContacts.Size = new System.Drawing.Size(61, 32);
            this.buttonSaveContacts.TabIndex = 13;
            this.buttonSaveContacts.Text = "Save";
            this.buttonSaveContacts.UseVisualStyleBackColor = false;
            this.buttonSaveContacts.Click += new System.EventHandler(this.buttonSaveContacts_Click);
            // 
            // listViewContacts
            // 
            this.listViewContacts.CheckBoxes = true;
            this.listViewContacts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnLogin,
            this.columnName,
            this.columnTag});
            this.listViewContacts.ContextMenuStrip = this.contextMenuStripContacts;
            this.listViewContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewContacts.FullRowSelect = true;
            this.listViewContacts.Location = new System.Drawing.Point(6, 26);
            this.listViewContacts.Name = "listViewContacts";
            this.listViewContacts.Size = new System.Drawing.Size(234, 399);
            this.listViewContacts.TabIndex = 0;
            this.listViewContacts.UseCompatibleStateImageBehavior = false;
            this.listViewContacts.View = System.Windows.Forms.View.Details;
            this.listViewContacts.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewContacts_ColumnClick);
            // 
            // columnLogin
            // 
            this.columnLogin.Text = "LOGIN";
            this.columnLogin.Width = 63;
            // 
            // columnName
            // 
            this.columnName.Text = "NAME";
            this.columnName.Width = 63;
            // 
            // columnTag
            // 
            this.columnTag.Text = "TAG";
            this.columnTag.Width = 71;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBoxFileName);
            this.groupBox3.Controls.Add(this.buttonSendFileAll);
            this.groupBox3.Location = new System.Drawing.Point(12, 484);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(524, 60);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SEND AND LOAD FILE";
            // 
            // buttonSendFileAll
            // 
            this.buttonSendFileAll.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSendFileAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSendFileAll.Location = new System.Drawing.Point(6, 21);
            this.buttonSendFileAll.Name = "buttonSendFileAll";
            this.buttonSendFileAll.Size = new System.Drawing.Size(120, 32);
            this.buttonSendFileAll.TabIndex = 16;
            this.buttonSendFileAll.Text = "SendAllFile";
            this.buttonSendFileAll.UseVisualStyleBackColor = false;
            this.buttonSendFileAll.Click += new System.EventHandler(this.buttonSendFileAll_Click);
            // 
            // visualStyler1
            // 
            this.visualStyler1.HookVisualStyles = true;
            this.visualStyler1.HostForm = this;
            this.visualStyler1.License = ((SkinSoft.VisualStyler.Licensing.VisualStylerLicense)(resources.GetObject("visualStyler1.License")));
            this.visualStyler1.LoadVisualStyle(null, "Office2007 (Black).vssf");
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxFileName.Location = new System.Drawing.Point(259, 21);
            this.textBoxFileName.Multiline = true;
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(259, 32);
            this.textBoxFileName.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(146, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "Receive File";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 554);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.listViewMessages);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonExit);
            this.Name = "Form1";
            this.Text = "MYOWNCHAT";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStripMessages.ResumeLayout(false);
            this.contextMenuStripContacts.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visualStyler1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.ListView listViewMessages;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripContacts;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listViewContacts;
        private System.Windows.Forms.ColumnHeader columnLogin;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.Button buttonLoadContacts;
        private System.Windows.Forms.Button buttonSaveContacts;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnTag;
        private System.Windows.Forms.CheckBox checkBoxGroups;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSendFileAll;
        private System.Windows.Forms.ListView listViewClients;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private SkinSoft.VisualStyler.VisualStyler visualStyler1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMessages;
        private System.Windows.Forms.ToolStripMenuItem sendPrivateMessageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendFileToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFileName;
    }
}

