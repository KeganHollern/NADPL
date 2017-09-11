using MaterialSkin;
using MaterialSkin.Controls;
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

    public enum NotificationType {
        Ok,
        YesNo,

    }


    public partial class Notification : MaterialForm {
        private NotificationType type;


        public delegate DialogResult ShowCallback(string Title, string Message, NotificationType type);
        public static DialogResult Show(string Title,string Message) {
            return Show(Title, Message, NotificationType.Ok);
        }
        public static DialogResult Show(string Title,string Message,NotificationType Type) {
            if(Form1.Instance != null) {
                if(Form1.Instance.InvokeRequired) {
                    ShowCallback callback = new ShowCallback(Show);
                    return (DialogResult)Form1.Instance.Invoke(callback, Title, Message, Type);
                }
            }
            Notification notify = new Notification();
            notify.SetNotificationInformation(Title, Message, Type);
            return notify.ShowDialog();
        }


        public Notification() {
            InitializeComponent();

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red800, Primary.Red900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        public void SetNotificationInformation(string Title, string Message, NotificationType Type) {
            this.Text = Title;
            this.type = Type;

            materialLabel1.Text = Message;
            if(this.type == NotificationType.Ok) {
                materialRaisedButton2.Visible = false;
                materialRaisedButton1.Text = "Okay";
            }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e) {
            this.DialogResult = this.type == NotificationType.Ok ? DialogResult.OK : DialogResult.Yes;
            this.Close();
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
