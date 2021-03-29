
namespace ClientChat
{
    partial class CreateGroup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNameGroup = new System.Windows.Forms.TextBox();
            this.listViewContacts = new System.Windows.Forms.ListView();
            this.columnHeaderLogin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCreateGroup = new System.Windows.Forms.Button();
            this.checkBoxAll = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(55, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "NAME GROUP";
            // 
            // textBoxNameGroup
            // 
            this.textBoxNameGroup.Location = new System.Drawing.Point(27, 39);
            this.textBoxNameGroup.Multiline = true;
            this.textBoxNameGroup.Name = "textBoxNameGroup";
            this.textBoxNameGroup.Size = new System.Drawing.Size(176, 27);
            this.textBoxNameGroup.TabIndex = 1;
            this.textBoxNameGroup.TextChanged += new System.EventHandler(this.textBoxNameGroup_TextChanged);
            // 
            // listViewContacts
            // 
            this.listViewContacts.CheckBoxes = true;
            this.listViewContacts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderLogin});
            this.listViewContacts.Location = new System.Drawing.Point(27, 100);
            this.listViewContacts.Name = "listViewContacts";
            this.listViewContacts.Size = new System.Drawing.Size(176, 187);
            this.listViewContacts.TabIndex = 2;
            this.listViewContacts.UseCompatibleStateImageBehavior = false;
            this.listViewContacts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderLogin
            // 
            this.columnHeaderLogin.Text = "LOGIN";
            this.columnHeaderLogin.Width = 145;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(55, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "MY COTNACTS";
            // 
            // buttonCreateGroup
            // 
            this.buttonCreateGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCreateGroup.Location = new System.Drawing.Point(12, 323);
            this.buttonCreateGroup.Name = "buttonCreateGroup";
            this.buttonCreateGroup.Size = new System.Drawing.Size(124, 33);
            this.buttonCreateGroup.TabIndex = 4;
            this.buttonCreateGroup.Text = "Create Group";
            this.buttonCreateGroup.UseVisualStyleBackColor = true;
            this.buttonCreateGroup.Click += new System.EventHandler(this.buttonCreateGroup_Click);
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxAll.Location = new System.Drawing.Point(69, 293);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(95, 21);
            this.checkBoxAll.TabIndex = 5;
            this.checkBoxAll.Text = "check All";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(142, 323);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CreateGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 368);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxAll);
            this.Controls.Add(this.buttonCreateGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewContacts);
            this.Controls.Add(this.textBoxNameGroup);
            this.Controls.Add(this.label1);
            this.Name = "CreateGroup";
            this.Text = "GroupChat";
            this.Load += new System.EventHandler(this.CreateGroup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNameGroup;
        private System.Windows.Forms.ListView listViewContacts;
        private System.Windows.Forms.ColumnHeader columnHeaderLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCreateGroup;
        private System.Windows.Forms.CheckBox checkBoxAll;
        private System.Windows.Forms.Button button1;
    }
}