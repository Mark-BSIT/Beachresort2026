namespace BeachResortAPIWinForm.Forms
{
    partial class Register
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.checkstaff = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkadmin = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Createaccount = new System.Windows.Forms.Button();
            this.conpass = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.logPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.logUser = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.checkstaff);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.checkadmin);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.Createaccount);
            this.panel1.Controls.Add(this.conpass);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.logPass);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.logUser);
            this.panel1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Location = new System.Drawing.Point(129, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(291, 359);
            this.panel1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(169, 267);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Staff";
            // 
            // checkstaff
            // 
            this.checkstaff.AutoSize = true;
            this.checkstaff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkstaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkstaff.Location = new System.Drawing.Point(151, 266);
            this.checkstaff.Name = "checkstaff";
            this.checkstaff.Size = new System.Drawing.Size(15, 14);
            this.checkstaff.TabIndex = 24;
            this.checkstaff.UseVisualStyleBackColor = true;
            this.checkstaff.CheckedChanged += new System.EventHandler(this.checkstaff_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(100, 267);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Admin";
            // 
            // checkadmin
            // 
            this.checkadmin.AutoSize = true;
            this.checkadmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkadmin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkadmin.Location = new System.Drawing.Point(82, 266);
            this.checkadmin.Name = "checkadmin";
            this.checkadmin.Size = new System.Drawing.Size(15, 14);
            this.checkadmin.TabIndex = 22;
            this.checkadmin.UseVisualStyleBackColor = true;
            this.checkadmin.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(78, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 23);
            this.label4.TabIndex = 20;
            this.label4.Text = "Create Account";
            // 
            // Createaccount
            // 
            this.Createaccount.BackColor = System.Drawing.Color.White;
            this.Createaccount.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Createaccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Createaccount.ForeColor = System.Drawing.Color.Black;
            this.Createaccount.Location = new System.Drawing.Point(103, 300);
            this.Createaccount.Name = "Createaccount";
            this.Createaccount.Size = new System.Drawing.Size(81, 36);
            this.Createaccount.TabIndex = 19;
            this.Createaccount.Text = "Create  account";
            this.Createaccount.UseVisualStyleBackColor = false;
            this.Createaccount.Click += new System.EventHandler(this.Login_Click);
            // 
            // conpass
            // 
            this.conpass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conpass.Location = new System.Drawing.Point(63, 224);
            this.conpass.Name = "conpass";
            this.conpass.Size = new System.Drawing.Size(191, 20);
            this.conpass.TabIndex = 17;
            this.conpass.UseSystemPasswordChar = true;
            // 
            // panel8
            // 
            this.panel8.BackgroundImage = global::BeachResortAPIWinForm.Properties.Resources.profile;
            this.panel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel8.Location = new System.Drawing.Point(32, 115);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(22, 30);
            this.panel8.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(62, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Confirm password";
            // 
            // logPass
            // 
            this.logPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logPass.Location = new System.Drawing.Point(63, 176);
            this.logPass.Name = "logPass";
            this.logPass.Size = new System.Drawing.Size(191, 20);
            this.logPass.TabIndex = 13;
            this.logPass.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(62, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(62, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Password:";
            // 
            // logUser
            // 
            this.logUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logUser.Location = new System.Drawing.Point(63, 125);
            this.logUser.Name = "logUser";
            this.logUser.Size = new System.Drawing.Size(191, 20);
            this.logUser.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::BeachResortAPIWinForm.Properties.Resources.reset_password__1_;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.Location = new System.Drawing.Point(32, 166);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(22, 30);
            this.panel2.TabIndex = 15;
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::BeachResortAPIWinForm.Properties.Resources.reset_password__1_;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel3.Location = new System.Drawing.Point(32, 214);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(22, 30);
            this.panel3.TabIndex = 15;
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(559, 426);
            this.Controls.Add(this.panel1);
            this.Name = "Register";
            this.Text = "Register";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Createaccount;
        private System.Windows.Forms.TextBox conpass;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox logPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox logUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkstaff;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkadmin;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}