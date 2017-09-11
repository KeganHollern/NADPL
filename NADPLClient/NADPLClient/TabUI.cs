using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace NADPLClient {
    class TabUI {

        public delegate void ShowCallback(int botNumber);

        public static void SetHeroes(int botNumber,string[] portraits) {
            if(portraits.Length == 0) {
                portraits = new string[] {
                    "no_hero",
                    "no_hero",
                    "no_hero",
                    "no_hero",
                    "no_hero",
                    "no_hero",
                    "no_hero",
                    "no_hero",
                    "no_hero",
                    "no_hero"
                };
            };
            Control[] ctrls = FindChildControls(Form1.Instance.tabPage3, "GamePanel" + (botNumber + 1).ToString());
            if (ctrls.Length == 0)
                return;
            Panel gamePanel = (Panel)ctrls[0];

            List<PictureBox> DireCtrls = FindChildControls(gamePanel, "DireHero").Cast<PictureBox>().ToList();
            List<PictureBox> RadiantCtrls = FindChildControls(gamePanel, "RadiantHero").Cast<PictureBox>().ToList();

            Bitmap no_hero = new Bitmap(32, 32, PixelFormat.Format24bppRgb);
            Color background = Color.FromArgb(255, 51, 51, 51);
            using (Graphics grp = Graphics.FromImage(no_hero)) {
                grp.Clear(background);
                no_hero.MakeTransparent(background);
            }

            int i = 0;
            foreach(string portrait in portraits) {
                if(i < 5) {
                    if (portrait == "no_hero") {
                        RadiantCtrls[i].Image = no_hero;
                    } else {
                        if (System.IO.File.Exists("data\\heroes\\" + portrait + ".png"))
                            RadiantCtrls[i].Image = Image.FromFile("data\\heroes\\" + portrait + ".png");
                        else
                            RadiantCtrls[i].Image = no_hero;
                    }
                } else {
                    if(portrait == "no_hero") {
                        DireCtrls[i - 5].Image = no_hero;
                    } else {
                        if(System.IO.File.Exists("data\\heroes\\" + portrait + ".png"))
                            DireCtrls[i - 5].Image = Image.FromFile("data\\heroes\\" + portrait + ".png");
                        else
                            DireCtrls[i - 5].Image = no_hero;
                    }
                }
                i++;
            }
        }

        public static void ShowLobby(int botNumber) {
            if(Form1.Instance.InvokeRequired) {
                ShowCallback callback = new ShowCallback(ShowLobby);
                Form1.Instance.Invoke(callback, botNumber);
                return;
            }


            if(botNumber == 0) {
                SetLobby1(true);
                SetGame1(false);
            } else if(botNumber == 1) {
                SetLobby2(true);
                SetGame2(false);
            } else if(botNumber == 2) {
                SetLobby3(true);
                SetGame3(false);
            } else {
                SetLobby4(true);
                SetGame4(false);
            }
        }
        public static void ShowGame(int botNumber) {
            if (Form1.Instance.InvokeRequired) {
                ShowCallback callback = new ShowCallback(ShowGame);
                Form1.Instance.Invoke(callback, botNumber);
                return;
            }

            if (botNumber == 0) {
                SetLobby1(false);
                SetGame1(true);
            } else if (botNumber == 1) {
                SetLobby2(false);
                SetGame2(true);
            } else if (botNumber == 2) {
                SetLobby3(false);
                SetGame3(true);
            } else {
                SetLobby4(false);
                SetGame4(true);
            }
        }

        public static Control[] FindChildControls(Control parent,string partialname) {
            List<Control> matching = new List<Control>();

            foreach(Control ctrl in parent.Controls) {
                if(ctrl.Name.ToLower().Contains(partialname.ToLower())) {
                    matching.Add(ctrl);
                }
            }

            if(matching.Count > 1) {
                matching.Sort((ctrl1, ctrl2) => {
                    return ctrl1.Name.CompareTo(ctrl2.Name);
                });
            }

            return matching.ToArray();
        }


        private static void SetLobby1(bool visible) {
            //set status
            Form1.Instance.LobbyLabel1.Visible = !visible;

            //set titles
            Form1.Instance.LobbyTitle1.Visible = visible;
            Form1.Instance.Radiant1.Visible = visible;
            Form1.Instance.Dire1.Visible = visible;

            //set buttons
            Form1.Instance.JoinRadiant1.Visible = visible;
            Form1.Instance.JoinDire1.Visible = visible;

            //set slots
            Control[] Slots = FindChildControls(Form1.Instance.LobbyPanel1, "RadiantSlot1");
            foreach (Control slotControl in Slots) {
                slotControl.Visible = visible;
            }
            Slots = FindChildControls(Form1.Instance.LobbyPanel1, "DireSlot1");
            foreach (Control slotControl in Slots) {
                slotControl.Visible = visible;
            }
        }
        private static void SetLobby2(bool visible) {
            //set status
            Form1.Instance.LobbyLabel2.Visible = !visible;

            //set titles
            Form1.Instance.LobbyTitle2.Visible = visible;
            Form1.Instance.Radiant2.Visible = visible;
            Form1.Instance.Dire2.Visible = visible;

            //set buttons
            Form1.Instance.JoinRadiant2.Visible = visible;
            Form1.Instance.JoinDire2.Visible = visible;

            //set slots
            Control[] Slots = FindChildControls(Form1.Instance.LobbyPanel2, "RadiantSlot2");
            foreach (Control slotControl in Slots) {
                slotControl.Visible = visible;
            }
            Slots = FindChildControls(Form1.Instance.LobbyPanel2, "DireSlot2");
            foreach (Control slotControl in Slots) {
                slotControl.Visible = visible;
            }
        }
        private static void SetLobby3(bool visible) {
            //set status
            Form1.Instance.LobbyLabel3.Visible = !visible;

            //set titles
            Form1.Instance.LobbyTitle3.Visible = visible;
            Form1.Instance.Radiant3.Visible = visible;
            Form1.Instance.Dire3.Visible = visible;

            //set buttons
            Form1.Instance.JoinRadiant3.Visible = visible;
            Form1.Instance.JoinDire3.Visible = visible;

            //set slots

            Control[] Slots = FindChildControls(Form1.Instance.LobbyPanel3, "RadiantSlot3");
            foreach (Control slotControl in Slots) {
                slotControl.Visible = visible;
            }
            Slots = FindChildControls(Form1.Instance.LobbyPanel3, "DireSlot3");
            foreach (Control slotControl in Slots) {
                slotControl.Visible = visible;
            }
        }
        private static void SetLobby4(bool visible) {
            //set status
            Form1.Instance.LobbyLabel4.Visible = !visible;

            //set titles
            Form1.Instance.LobbyTitle4.Visible = visible;
            Form1.Instance.Radiant4.Visible = visible;
            Form1.Instance.Dire4.Visible = visible;

            //set buttons
            Form1.Instance.JoinRadiant4.Visible = visible;
            Form1.Instance.JoinDire4.Visible = visible;

            //set slots
            Control[] Slots = FindChildControls(Form1.Instance.LobbyPanel4, "RadiantSlot4");
            foreach (Control slotControl in Slots) {
                slotControl.Visible = visible;
            }
            Slots = FindChildControls(Form1.Instance.LobbyPanel4, "DireSlot4");
            foreach (Control slotControl in Slots) {
                slotControl.Visible = visible;
            }
        }
        private static void SetGame1(bool visible) {
            //set status
            Form1.Instance.GameLabel1.Visible = !visible;

            //set titles
            Form1.Instance.GameTitle1.Visible = visible;
            Form1.Instance.RadiantGameLabel1.Visible = visible;
            Form1.Instance.DireGameLabel1.Visible = visible;

            //set heroes
            Control[] HeroIcons = FindChildControls(Form1.Instance.GamePanel1, "RadiantHero1");
            foreach (Control slotControl in HeroIcons) {
                slotControl.Visible = visible;
            }
            HeroIcons = FindChildControls(Form1.Instance.GamePanel1, "DireHero1");
            foreach (Control slotControl in HeroIcons) {
                slotControl.Visible = visible;
            }

            //set players
            Control[] Players = FindChildControls(Form1.Instance.GamePanel1, "RadiantPlayer1");
            foreach (Control slotControl in Players) {
                slotControl.Visible = visible;
            }
            Players = FindChildControls(Form1.Instance.GamePanel1, "DirePlayer1");
            foreach (Control slotControl in Players) {
                slotControl.Visible = visible;
            }
        }
        private static void SetGame2(bool visible) {
            //set status
            Form1.Instance.GameLabel2.Visible = !visible;

            //set titles
            Form1.Instance.GameTitle2.Visible = visible;
            Form1.Instance.RadiantGameLabel2.Visible = visible;
            Form1.Instance.DireGameLabel2.Visible = visible;

            //set heroes
            Control[] HeroIcons = FindChildControls(Form1.Instance.GamePanel2, "RadiantHero2");
            foreach (Control slotControl in HeroIcons) {
                slotControl.Visible = visible;
            }
            HeroIcons = FindChildControls(Form1.Instance.GamePanel2, "DireHero2");
            foreach (Control slotControl in HeroIcons) {
                slotControl.Visible = visible;
            }

            //set players
            Control[] Players = FindChildControls(Form1.Instance.GamePanel2, "RadiantPlayer2");
            foreach (Control slotControl in Players) {
                slotControl.Visible = visible;
            }
            Players = FindChildControls(Form1.Instance.GamePanel2, "DirePlayer2");
            foreach (Control slotControl in Players) {
                slotControl.Visible = visible;
            }
        }
        private static void SetGame3(bool visible) {
            //set status
            Form1.Instance.GameLabel3.Visible = !visible;

            //set titles
            Form1.Instance.GameTitle3.Visible = visible;
            Form1.Instance.RadiantGameLabel3.Visible = visible;
            Form1.Instance.DireGameLabel3.Visible = visible;

            //set heroes
            Control[] HeroIcons = FindChildControls(Form1.Instance.GamePanel3, "RadiantHero3");
            foreach (Control slotControl in HeroIcons) {
                slotControl.Visible = visible;
            }
            HeroIcons = FindChildControls(Form1.Instance.GamePanel3, "DireHero3");
            foreach (Control slotControl in HeroIcons) {
                slotControl.Visible = visible;
            }

            //set players
            Control[] Players = FindChildControls(Form1.Instance.GamePanel3, "RadiantPlayer3");
            foreach (Control slotControl in Players) {
                slotControl.Visible = visible;
            }
            Players = FindChildControls(Form1.Instance.GamePanel3, "DirePlayer3");
            foreach (Control slotControl in Players) {
                slotControl.Visible = visible;
            }
        }
        private static void SetGame4(bool visible) {
            //set status
            Form1.Instance.GameLabel4.Visible = !visible;

            //set titles
            Form1.Instance.GameTitle4.Visible = visible;
            Form1.Instance.RadiantGameLabel4.Visible = visible;
            Form1.Instance.DireGameLabel4.Visible = visible;

            //set heroes
            Control[] HeroIcons = FindChildControls(Form1.Instance.GamePanel4, "RadiantHero4");
            foreach (Control slotControl in HeroIcons) {
                slotControl.Visible = visible;
            }
            HeroIcons = FindChildControls(Form1.Instance.GamePanel4, "DireHero4");
            foreach (Control slotControl in HeroIcons) {
                slotControl.Visible = visible;
            }

            //set players
            Control[] Players = FindChildControls(Form1.Instance.GamePanel4, "RadiantPlayer4");
            foreach (Control slotControl in Players) {
                slotControl.Visible = visible;
            }
            Players = FindChildControls(Form1.Instance.GamePanel4, "DirePlayer4");
            foreach (Control slotControl in Players) {
                slotControl.Visible = visible;
            }
        }


    }
}
