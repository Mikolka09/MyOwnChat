
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
            this.buttonExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.listViewMessages = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCancelPrivateChat = new System.Windows.Forms.Button();
            this.buttonInquiry = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
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
            this.checkBoxAll = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLoginFile = new System.Windows.Forms.TextBox();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.buttonSendFile = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExit.Location = new System.Drawing.Point(696, 468);
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
            this.label1.Location = new System.Drawing.Point(143, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "WINDOW MESSAGES CHAT";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxMessage.Location = new System.Drawing.Point(12, 33);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(253, 52);
            this.textBoxMessage.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(55, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "WRITE MESSAGE";
            // 
            // buttonSend
            // 
            this.buttonSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSend.Location = new System.Drawing.Point(289, 41);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(91, 36);
            this.buttonSend.TabIndex = 7;
            this.buttonSend.Text = "SEND";
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // listViewMessages
            // 
            this.listViewMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewMessages.Location = new System.Drawing.Point(12, 123);
            this.listViewMessages.Name = "listViewMessages";
            this.listViewMessages.Size = new System.Drawing.Size(524, 311);
            this.listViewMessages.TabIndex = 9;
            this.listViewMessages.UseCompatibleStateImageBehavior = false;
            this.listViewMessages.View = System.Windows.Forms.View.List;
            this.listViewMessages.DoubleClick += new System.EventHandler(this.toolStripMenuItemAdd_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.addToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 76);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCancelPrivateChat);
            this.groupBox1.Controls.Add(this.buttonInquiry);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Location = new System.Drawing.Point(447, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 87);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RRIVATE MESSAGE";
            // 
            // buttonCancelPrivateChat
            // 
            this.buttonCancelPrivateChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonCancelPrivateChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCancelPrivateChat.Location = new System.Drawing.Point(256, 45);
            this.buttonCancelPrivateChat.Name = "buttonCancelPrivateChat";
            this.buttonCancelPrivateChat.Size = new System.Drawing.Size(76, 32);
            this.buttonCancelPrivateChat.TabIndex = 12;
            this.buttonCancelPrivateChat.Text = "Cancel";
            this.buttonCancelPrivateChat.UseVisualStyleBackColor = false;
            this.buttonCancelPrivateChat.Click += new System.EventHandler(this.buttonCancelPrivateChat_Click);
            // 
            // buttonInquiry
            // 
            this.buttonInquiry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.buttonInquiry.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonInquiry.Location = new System.Drawing.Point(174, 45);
            this.buttonInquiry.Name = "buttonInquiry";
            this.buttonInquiry.Size = new System.Drawing.Size(76, 32);
            this.buttonInquiry.TabIndex = 11;
            this.buttonInquiry.Text = "Inquiry";
            this.buttonInquiry.UseVisualStyleBackColor = false;
            this.buttonInquiry.Click += new System.EventHandler(this.buttonInquiry_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(40, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Enter Login";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(6, 45);
            this.textBoxName.Multiline = true;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(162, 32);
            this.textBoxName.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listViewClients);
            this.groupBox2.Controls.Add(this.checkBoxGroups);
            this.groupBox2.Controls.Add(this.buttonLoadContacts);
            this.groupBox2.Controls.Add(this.buttonSaveContacts);
            this.groupBox2.Controls.Add(this.listViewContacts);
            this.groupBox2.Location = new System.Drawing.Point(542, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(246, 329);
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
            this.listViewClients.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewClients.Location = new System.Drawing.Point(6, 21);
            this.listViewClients.Name = "listViewClients";
            this.listViewClients.Size = new System.Drawing.Size(234, 254);
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
            this.checkBoxGroups.Location = new System.Drawing.Point(154, 295);
            this.checkBoxGroups.Name = "checkBoxGroups";
            this.checkBoxGroups.Size = new System.Drawing.Size(83, 21);
            this.checkBoxGroups.TabIndex = 15;
            this.checkBoxGroups.Text = "Groups";
            this.checkBoxGroups.UseVisualStyleBackColor = true;
            this.checkBoxGroups.CheckedChanged += new System.EventHandler(this.checkBoxGroups_CheckedChanged);
            // 
            // buttonLoadContacts
            // 
            this.buttonLoadContacts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonLoadContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLoadContacts.Location = new System.Drawing.Point(79, 289);
            this.buttonLoadContacts.Name = "buttonLoadContacts";
            this.buttonLoadContacts.Size = new System.Drawing.Size(65, 32);
            this.buttonLoadContacts.TabIndex = 14;
            this.buttonLoadContacts.Text = "Load";
            this.buttonLoadContacts.UseVisualStyleBackColor = false;
            this.buttonLoadContacts.Click += new System.EventHandler(this.buttonLoadContacts_Click);
            // 
            // buttonSaveContacts
            // 
            this.buttonSaveContacts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonSaveContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSaveContacts.Location = new System.Drawing.Point(12, 288);
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
            this.listViewContacts.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewContacts.FullRowSelect = true;
            this.listViewContacts.Location = new System.Drawing.Point(6, 22);
            this.listViewContacts.Name = "listViewContacts";
            this.listViewContacts.Size = new System.Drawing.Size(234, 254);
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
            this.groupBox3.Controls.Add(this.checkBoxAll);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBoxFileName);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBoxLoginFile);
            this.groupBox3.Controls.Add(this.buttonLoadFile);
            this.groupBox3.Controls.Add(this.buttonSendFile);
            this.groupBox3.Location = new System.Drawing.Point(13, 440);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(642, 64);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SEND AND LOAD FILE";
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxAll.Location = new System.Drawing.Point(272, 34);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(58, 21);
            this.checkBoxAll.TabIndex = 20;
            this.checkBoxAll.Text = "ALL";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(360, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "Receive File Name";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxFileName.Location = new System.Drawing.Point(345, 26);
            this.textBoxFileName.Multiline = true;
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(178, 32);
            this.textBoxFileName.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(110, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Login:";
            // 
            // textBoxLoginFile
            // 
            this.textBoxLoginFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxLoginFile.Location = new System.Drawing.Point(169, 26);
            this.textBoxLoginFile.Multiline = true;
            this.textBoxLoginFile.Name = "textBoxLoginFile";
            this.textBoxLoginFile.Size = new System.Drawing.Size(97, 32);
            this.textBoxLoginFile.TabIndex = 13;
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.BackColor = System.Drawing.Color.HotPink;
            this.buttonLoadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLoadFile.Location = new System.Drawing.Point(529, 26);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(103, 32);
            this.buttonLoadFile.TabIndex = 17;
            this.buttonLoadFile.Text = "LoadFile";
            this.buttonLoadFile.UseVisualStyleBackColor = false;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // buttonSendFile
            // 
            this.buttonSendFile.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonSendFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSendFile.Location = new System.Drawing.Point(6, 26);
            this.buttonSendFile.Name = "buttonSendFile";
            this.buttonSendFile.Size = new System.Drawing.Size(94, 32);
            this.buttonSendFile.TabIndex = 16;
            this.buttonSendFile.Text = "SendFile";
            this.buttonSendFile.UseVisualStyleBackColor = false;
            this.buttonSendFile.Click += new System.EventHandler(this.buttonSendFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 516);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listViewMessages);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonExit);
            this.Name = "Form1";
            this.Text = "MYOWNCHAT";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.ListView listViewMessages;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonInquiry;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonCancelPrivateChat;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLoginFile;
        private System.Windows.Forms.Button buttonLoadFile;
        private System.Windows.Forms.Button buttonSendFile;
        private System.Windows.Forms.CheckBox checkBoxAll;
        private System.Windows.Forms.ListView listViewClients;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

