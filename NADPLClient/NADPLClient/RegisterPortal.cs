using MaterialSkin;
using MaterialSkin.Controls;
using NADPLClient.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NADPLClient {
    public partial class RegisterPortal : MaterialForm {
        Form1 mainForm;
        public RegisterPortal(Form1 mainForm,bool badconnection,bool baduser) {
            this.mainForm = mainForm;
            
            InitializeComponent();

            if (badconnection) {
                this.ErrorLabel.Visible = true;
                this.ErrorLabel.Text = "Bad Network Connection";
            }
            if (baduser) {
                this.ErrorLabel.Visible = true;
                this.ErrorLabel.Text = "That User Is Taken";
            }

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red800, Primary.Red900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialFlatButton1_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e) {
            string user = userText.Text;
            string pass = passText.Text;
            string pass2 = passText2.Text;
            string steamid64 = steamId64text.Text;

            if(user.Length < 4) {
                Notification.Show("Error", "Username must be > 4 characters.");
                return;
            }
            if(pass.Length < 6) {
                Notification.Show("Error", "Password must be > 6 characters.");
                return;
            }
            if (user.Length > 12) {
                Notification.Show("Error", "Username must be < 12 characters.");
                return;
            }
            if (pass.Length > 16) {
                Notification.Show("Error", "Password must be < 16 characters.");
                return;
            }


            ServerHandler handler = mainForm.getServerHandler();

            ulong id = 0;
            if(!ulong.TryParse(steamid64,out id)) {
                //TODO: bad steam id
                Notification.Show("Error", "Improper SteamID input");
                return;
            }

            if(pass != pass2) {
                //TODO: password mismatch
                Notification.Show("Error", "Password Mismatch");
                return;
            }

            //Register

            if (handler == null) {
                //No network object
                this.DialogResult = DialogResult.No;

            } else if (handler.Register(user, pass, id)) {
                //success
                this.DialogResult = DialogResult.OK;
            } else if (!handler.Connected) {
                //Not connected
                this.DialogResult = DialogResult.No;
            } else {
                //username taken
                this.DialogResult = DialogResult.Retry;
            }

            this.Close();
        }
    }
}
