namespace NADPLClient {
    partial class RegisterPortal {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterPortal));
            this.materialFlatButton1 = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialRaisedButton1 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.passText2 = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.userText = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.steamId64text = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.passText = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.ErrorLabel = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // materialFlatButton1
            // 
            this.materialFlatButton1.AutoSize = true;
            this.materialFlatButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton1.Depth = 0;
            this.materialFlatButton1.Location = new System.Drawing.Point(13, 193);
            this.materialFlatButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton1.Name = "materialFlatButton1";
            this.materialFlatButton1.Primary = false;
            this.materialFlatButton1.Size = new System.Drawing.Size(64, 36);
            this.materialFlatButton1.TabIndex = 8;
            this.materialFlatButton1.Text = "Return";
            this.materialFlatButton1.UseVisualStyleBackColor = true;
            this.materialFlatButton1.Click += new System.EventHandler(this.materialFlatButton1_Click);
            // 
            // materialRaisedButton1
            // 
            this.materialRaisedButton1.Depth = 0;
            this.materialRaisedButton1.Location = new System.Drawing.Point(198, 199);
            this.materialRaisedButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton1.Name = "materialRaisedButton1";
            this.materialRaisedButton1.Primary = true;
            this.materialRaisedButton1.Size = new System.Drawing.Size(90, 26);
            this.materialRaisedButton1.TabIndex = 7;
            this.materialRaisedButton1.Text = "Register";
            this.materialRaisedButton1.UseVisualStyleBackColor = true;
            this.materialRaisedButton1.Click += new System.EventHandler(this.materialRaisedButton1_Click);
            // 
            // passText2
            // 
            this.passText2.Depth = 0;
            this.passText2.Hint = "Password (again)";
            this.passText2.Location = new System.Drawing.Point(12, 134);
            this.passText2.MouseState = MaterialSkin.MouseState.HOVER;
            this.passText2.Name = "passText2";
            this.passText2.PasswordChar = '*';
            this.passText2.SelectedText = "";
            this.passText2.SelectionLength = 0;
            this.passText2.SelectionStart = 0;
            this.passText2.Size = new System.Drawing.Size(276, 23);
            this.passText2.TabIndex = 6;
            this.passText2.UseSystemPasswordChar = false;
            // 
            // userText
            // 
            this.userText.Depth = 0;
            this.userText.Hint = "Username";
            this.userText.Location = new System.Drawing.Point(12, 76);
            this.userText.MouseState = MaterialSkin.MouseState.HOVER;
            this.userText.Name = "userText";
            this.userText.PasswordChar = '\0';
            this.userText.SelectedText = "";
            this.userText.SelectionLength = 0;
            this.userText.SelectionStart = 0;
            this.userText.Size = new System.Drawing.Size(276, 23);
            this.userText.TabIndex = 5;
            this.userText.UseSystemPasswordChar = false;
            // 
            // steamId64text
            // 
            this.steamId64text.Depth = 0;
            this.steamId64text.Hint = "SteamID64 (THIS MUST BE CORRECT)";
            this.steamId64text.Location = new System.Drawing.Point(12, 163);
            this.steamId64text.MouseState = MaterialSkin.MouseState.HOVER;
            this.steamId64text.Name = "steamId64text";
            this.steamId64text.PasswordChar = '\0';
            this.steamId64text.SelectedText = "";
            this.steamId64text.SelectionLength = 0;
            this.steamId64text.SelectionStart = 0;
            this.steamId64text.Size = new System.Drawing.Size(276, 23);
            this.steamId64text.TabIndex = 9;
            this.steamId64text.UseSystemPasswordChar = false;
            // 
            // passText
            // 
            this.passText.Depth = 0;
            this.passText.Hint = "Password";
            this.passText.Location = new System.Drawing.Point(13, 105);
            this.passText.MouseState = MaterialSkin.MouseState.HOVER;
            this.passText.Name = "passText";
            this.passText.PasswordChar = '*';
            this.passText.SelectedText = "";
            this.passText.SelectionLength = 0;
            this.passText.SelectionStart = 0;
            this.passText.Size = new System.Drawing.Size(276, 23);
            this.passText.TabIndex = 10;
            this.passText.UseSystemPasswordChar = false;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Depth = 0;
            this.ErrorLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.ErrorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ErrorLabel.Location = new System.Drawing.Point(8, 0);
            this.ErrorLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(176, 19);
            this.ErrorLabel.TabIndex = 11;
            this.ErrorLabel.Text = "Bad Network Connection";
            this.ErrorLabel.Visible = false;
            // 
            // RegisterPortal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 231);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.passText);
            this.Controls.Add(this.steamId64text);
            this.Controls.Add(this.materialFlatButton1);
            this.Controls.Add(this.materialRaisedButton1);
            this.Controls.Add(this.passText2);
            this.Controls.Add(this.userText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RegisterPortal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NA Dota 2 Pleb League";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialFlatButton materialFlatButton1;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton1;
        private MaterialSkin.Controls.MaterialSingleLineTextField passText2;
        private MaterialSkin.Controls.MaterialSingleLineTextField userText;
        private MaterialSkin.Controls.MaterialSingleLineTextField steamId64text;
        private MaterialSkin.Controls.MaterialSingleLineTextField passText;
        private MaterialSkin.Controls.MaterialLabel ErrorLabel;
    }
}