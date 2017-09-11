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
    public partial class LoginPortal : MaterialForm {
        private Form1 mainForm;

        public LoginPortal(Form1 mainForm, bool badconnection, bool baduser) {
            this.mainForm = mainForm;
            InitializeComponent();

            if (badconnection) {
                this.ErrorLabel.Visible = true;
                this.ErrorLabel.Text = "Bad Network Connection";
            }
            if (baduser) {
                this.ErrorLabel.Visible = true;
                this.ErrorLabel.Text = "Incorrect Login.";
            }

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red800, Primary.Red900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e) {

            string user = materialSingleLineTextField1.Text;
            string pass = materialSingleLineTextField2.Text;

            if (user.Length < 4) {
                Notification.Show("Error", "Incorrect Username Format.");
                return;
            }
            if (pass.Length < 6) {
                Notification.Show("Error", "Incorrect Password Format.");
                return;
            }
            if (user.Length > 12) {
                Notification.Show("Error", "Incorrect Username Format.");
                return;
            }
            if (pass.Length > 16) {
                Notification.Show("Error", "Incorrect Password Format.");
                return;
            }

            ServerHandler handler = mainForm.getServerHandler();

            if (handler == null) {
                //No network object
                this.DialogResult = DialogResult.No;

            } else if (handler.Login(user, pass)) {
                //success
                this.DialogResult = DialogResult.OK;
            } else if (!handler.Connected) {
                //Not connected
                this.DialogResult = DialogResult.No;
            } else {
                //failed login
                this.DialogResult = DialogResult.Retry;
            }
            
            this.Close();
        }

        private void materialFlatButton1_Click(object sender, EventArgs e) {
            bool success = false;
            bool baduser = false;
            bool badnetwork = false;
            while(true) {
                RegisterPortal portal = new RegisterPortal(mainForm, badnetwork, baduser);
                badnetwork = false;
                baduser = false;

                DialogResult result = portal.ShowDialog();
                if(result == DialogResult.Cancel) {
                    //exiting without any result
                    break;
                }
                if(result == DialogResult.OK) {
                    //exiting with success;
                    success = true;
                    break;

                } else if(result == DialogResult.No) {
                    badnetwork = true;
                } else if(result == DialogResult.Retry) {
                    baduser = true;
                }

                portal.Dispose();
            }
            if (success) {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
