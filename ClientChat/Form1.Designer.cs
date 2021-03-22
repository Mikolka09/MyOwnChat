
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
            this.buttonExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.listViewMessages = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonInquiry = new System.Windows.Forms.Button();
            this.buttonCancelPrivateChat = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExit.Location = new System.Drawing.Point(697, 453);
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
            this.label1.Location = new System.Drawing.Point(286, 102);
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
            this.listViewMessages.Location = new System.Drawing.Point(12, 123);
            this.listViewMessages.Name = "listViewMessages";
            this.listViewMessages.Size = new System.Drawing.Size(776, 324);
            this.listViewMessages.TabIndex = 9;
            this.listViewMessages.UseCompatibleStateImageBehavior = false;
            this.listViewMessages.View = System.Windows.Forms.View.List;
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
            this.groupBox1.Text = "RRIVATE CHAT";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(6, 45);
            this.textBoxName.Multiline = true;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(162, 32);
            this.textBoxName.TabIndex = 0;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 501);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}

