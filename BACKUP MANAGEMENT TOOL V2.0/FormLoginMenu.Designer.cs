namespace BackupManagementTool
{
    partial class FormLoginMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoginMenu));
            textBoxPassword = new TextBox();
            textBoxUserName = new TextBox();
            labelUserName = new Label();
            labelPassword = new Label();
            buttonLogIn = new Button();
            SuspendLayout();
            // 
            // textBoxPassword
            // 
            textBoxPassword.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            textBoxPassword.Location = new Point(45, 148);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new Size(150, 29);
            textBoxPassword.TabIndex = 1;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxUserName
            // 
            textBoxUserName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            textBoxUserName.Location = new Point(45, 94);
            textBoxUserName.Name = "textBoxUserName";
            textBoxUserName.Size = new Size(150, 29);
            textBoxUserName.TabIndex = 0;
            // 
            // labelUserName
            // 
            labelUserName.AutoSize = true;
            labelUserName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelUserName.Location = new Point(200, 94);
            labelUserName.Name = "labelUserName";
            labelUserName.Size = new Size(101, 25);
            labelUserName.TabIndex = 2;
            labelUserName.Text = "Username";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelPassword.Location = new Point(201, 148);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(97, 25);
            labelPassword.TabIndex = 3;
            labelPassword.Text = "Password";
            // 
            // buttonLogIn
            // 
            buttonLogIn.BackColor = SystemColors.Info;
            buttonLogIn.Location = new Point(101, 244);
            buttonLogIn.Name = "buttonLogIn";
            buttonLogIn.Size = new Size(123, 49);
            buttonLogIn.TabIndex = 2;
            buttonLogIn.Text = "Log in";
            buttonLogIn.UseVisualStyleBackColor = false;
            buttonLogIn.Click += ButtonLogIn_Click;
            // 
            // FormLoginMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Ivory;
            ClientSize = new Size(322, 346);
            Controls.Add(buttonLogIn);
            Controls.Add(labelPassword);
            Controls.Add(labelUserName);
            Controls.Add(textBoxUserName);
            Controls.Add(textBoxPassword);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLoginMenu";
            Text = "Login Menu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxPassword;
        private TextBox textBoxUserName;
        private Label labelUserName;
        private Label labelPassword;
        private Button buttonLogIn;
    }
}
