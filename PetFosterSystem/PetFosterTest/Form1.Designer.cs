namespace PetFoster.Test
{
    partial class Form1
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
            this.UIDLabel = new System.Windows.Forms.Label();
            this.passwdlabel = new System.Windows.Forms.Label();
            this.UIDBox = new System.Windows.Forms.TextBox();
            this.PwdBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.petbutton = new System.Windows.Forms.Button();
            this.petAvatar = new System.Windows.Forms.PictureBox();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.Unregisterbutton = new System.Windows.Forms.Button();
            this.Supervisebutton = new System.Windows.Forms.Button();
            this.Chpasswdbutton = new System.Windows.Forms.Button();
            this.VetViewButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.petAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // UIDLabel
            // 
            this.UIDLabel.AutoSize = true;
            this.UIDLabel.Location = new System.Drawing.Point(206, 129);
            this.UIDLabel.Name = "UIDLabel";
            this.UIDLabel.Size = new System.Drawing.Size(52, 15);
            this.UIDLabel.TabIndex = 0;
            this.UIDLabel.Text = "用户名";
            // 
            // passwdlabel
            // 
            this.passwdlabel.AutoSize = true;
            this.passwdlabel.Location = new System.Drawing.Point(209, 158);
            this.passwdlabel.Name = "passwdlabel";
            this.passwdlabel.Size = new System.Drawing.Size(37, 15);
            this.passwdlabel.TabIndex = 1;
            this.passwdlabel.Text = "密码";
            // 
            // UIDBox
            // 
            this.UIDBox.Location = new System.Drawing.Point(286, 118);
            this.UIDBox.Name = "UIDBox";
            this.UIDBox.Size = new System.Drawing.Size(100, 25);
            this.UIDBox.TabIndex = 2;
            // 
            // PwdBox
            // 
            this.PwdBox.Location = new System.Drawing.Point(286, 158);
            this.PwdBox.Name = "PwdBox";
            this.PwdBox.Size = new System.Drawing.Size(100, 25);
            this.PwdBox.TabIndex = 3;
            this.PwdBox.TextChanged += new System.EventHandler(this.PwdBox_TextChanged);
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(303, 213);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "登录";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // petbutton
            // 
            this.petbutton.Location = new System.Drawing.Point(474, 212);
            this.petbutton.Name = "petbutton";
            this.petbutton.Size = new System.Drawing.Size(75, 23);
            this.petbutton.TabIndex = 5;
            this.petbutton.Text = "宠物测试";
            this.petbutton.UseVisualStyleBackColor = true;
            this.petbutton.Click += new System.EventHandler(this.petbutton_Click);
            // 
            // petAvatar
            // 
            this.petAvatar.Location = new System.Drawing.Point(593, 201);
            this.petAvatar.Name = "petAvatar";
            this.petAvatar.Size = new System.Drawing.Size(195, 196);
            this.petAvatar.TabIndex = 6;
            this.petAvatar.TabStop = false;
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(303, 261);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(75, 23);
            this.RegisterButton.TabIndex = 7;
            this.RegisterButton.Text = "注册";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // Unregisterbutton
            // 
            this.Unregisterbutton.BackColor = System.Drawing.SystemColors.Info;
            this.Unregisterbutton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Unregisterbutton.Location = new System.Drawing.Point(452, 261);
            this.Unregisterbutton.Name = "Unregisterbutton";
            this.Unregisterbutton.Size = new System.Drawing.Size(75, 23);
            this.Unregisterbutton.TabIndex = 8;
            this.Unregisterbutton.Text = "注销";
            this.Unregisterbutton.UseVisualStyleBackColor = false;
            this.Unregisterbutton.Click += new System.EventHandler(this.Unregisterbutton_Click);
            // 
            // Supervisebutton
            // 
            this.Supervisebutton.Location = new System.Drawing.Point(310, 342);
            this.Supervisebutton.Name = "Supervisebutton";
            this.Supervisebutton.Size = new System.Drawing.Size(75, 21);
            this.Supervisebutton.TabIndex = 9;
            this.Supervisebutton.Text = "封号";
            this.Supervisebutton.UseVisualStyleBackColor = true;
            this.Supervisebutton.Click += new System.EventHandler(this.Supervisebutton_Click);
            // 
            // Chpasswdbutton
            // 
            this.Chpasswdbutton.Location = new System.Drawing.Point(452, 342);
            this.Chpasswdbutton.Name = "Chpasswdbutton";
            this.Chpasswdbutton.Size = new System.Drawing.Size(75, 23);
            this.Chpasswdbutton.TabIndex = 10;
            this.Chpasswdbutton.Text = "忘记密码";
            this.Chpasswdbutton.UseVisualStyleBackColor = true;
            this.Chpasswdbutton.Click += new System.EventHandler(this.Chpasswdbutton_Click);
            // 
            // VetViewButton
            // 
            this.VetViewButton.Location = new System.Drawing.Point(84, 140);
            this.VetViewButton.Name = "VetViewButton";
            this.VetViewButton.Size = new System.Drawing.Size(75, 23);
            this.VetViewButton.TabIndex = 11;
            this.VetViewButton.Text = "查看用户信息";
            this.VetViewButton.UseVisualStyleBackColor = true;
            this.VetViewButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.VetViewButton);
            this.Controls.Add(this.Chpasswdbutton);
            this.Controls.Add(this.Supervisebutton);
            this.Controls.Add(this.Unregisterbutton);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.petAvatar);
            this.Controls.Add(this.petbutton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.PwdBox);
            this.Controls.Add(this.UIDBox);
            this.Controls.Add(this.passwdlabel);
            this.Controls.Add(this.UIDLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.petAvatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UIDLabel;
        private System.Windows.Forms.Label passwdlabel;
        private System.Windows.Forms.TextBox UIDBox;
        private System.Windows.Forms.TextBox PwdBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button petbutton;
        private System.Windows.Forms.PictureBox petAvatar;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.Button Unregisterbutton;
        private System.Windows.Forms.Button Supervisebutton;
        private System.Windows.Forms.Button Chpasswdbutton;
        private System.Windows.Forms.Button VetViewButton;
    }
}

