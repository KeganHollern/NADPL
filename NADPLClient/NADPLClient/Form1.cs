using ChatSharp;
using MaterialSkin;
using MaterialSkin.Controls;
using NADPLClient.Account;
using NADPLClient.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NADPLClient {
    public partial class Form1 : MaterialForm {
        public static Form1 Instance;
        private IrcClient chat;
        private ServerHandler server;
        private bool canChat;
        private bool closing;
        private Thread TabManageThread;
        

        public ServerHandler getServerHandler() {
            return server;
        }


        public Form1() {
            Form1.Instance = this;
            closing = false;
            InitializeComponent();

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red800, Primary.Red900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            this.richTextBox1.Font = materialSkinManager.ROBOTO_MEDIUM_11;
            this.richTextBox2.Font = materialSkinManager.ROBOTO_MEDIUM_11;

            this.AcceptButton = materialRaisedButton1;
        }


        private void materialRaisedButton1_Click(object sender, EventArgs e) {
            if (materialSingleLineTextField1.Text != "") {
                if (chat == null)
                    return;
                if (!canChat)
                    return;

                chat.SendMessage(materialSingleLineTextField1.Text, "#nadpl_chat");
                AddUserChat(UserAccount.Information.user, materialSingleLineTextField1.Text);
                materialSingleLineTextField1.Text = "";
            }
        }

        private void InitIRC() {
            canChat = false;
            chat = new IrcClient("irc.freenode.net", new IrcUser(UserAccount.Information.user, UserAccount.Information.user));
            chat.ConnectionComplete += Chat_ConnectionComplete;
            chat.ChannelMessageRecieved += Chat_ChannelMessageRecieved;
            chat.NetworkError += Chat_NetworkError;
            
            chat.ConnectAsync();
            updateChatBox("Connecting to chat...");
        }

        private void Chat_NetworkError(object sender, ChatSharp.Events.SocketErrorEventArgs e) {
            //--- TODO: Handle chat errors
        }

        private void Chat_ConnectionComplete(object sender, EventArgs e) {
            canChat = true;
            chat.JoinChannel("#nadpl_chat");
            updateChatBox("Connected!");
        }

        private delegate void updateChatBoxCallback(string text);
        private void updateChatBox(string text) {

            if (richTextBox1.InvokeRequired) {
                updateChatBoxCallback callback = new updateChatBoxCallback(updateChatBox);
                richTextBox1.Invoke(callback,text);
                return;
            }
            richTextBox1.Text = text;
        }

        private delegate void AddUserChatCallback(string name, string msg);
        private void AddUserChat(string name,string msg) {
            if(richTextBox1.InvokeRequired) {
                AddUserChatCallback callback = new AddUserChatCallback(AddUserChat);
                richTextBox1.Invoke(callback, name, msg);
                return;
            }
            this.richTextBox1.Text += "\n<" + DateTime.Now.ToString("h:mm tt") + "> " + name + ": " + msg;
            this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
            this.richTextBox1.ScrollToCaret();
        }

        private void Chat_ChannelMessageRecieved(object sender, ChatSharp.Events.PrivateMessageEventArgs e) {
            IrcMessage msg = e.IrcMessage;
            if(msg.Parameters.Length >= 2) {
                string name = msg.RawMessage.Split('!')[0].Substring(1);
                string message = msg.Parameters[1];

                AddUserChat(name, message);
            }
        }

        private void BotDataReceived() {
            LobbyLoadingLabel.Visible = false;
            GameLoadingLabel.Visible = false;

            GamePanel1.Visible = true;
            GamePanel2.Visible = true;
            GamePanel3.Visible = true;
            GamePanel4.Visible = true;

            LobbyPanel1.Visible = true;
            LobbyPanel2.Visible = true;
            LobbyPanel3.Visible = true;
            LobbyPanel4.Visible = true;
        }
        private void InitUI() {
            GameLabel1.Visible = false;
            GameLabel2.Visible = false;
            GameLabel3.Visible = false;
            GameLabel4.Visible = false;
            GamePanel1.Visible = false;
            GamePanel2.Visible = false;
            GamePanel3.Visible = false;
            GamePanel4.Visible = false;


            LobbyLabel1.Visible = false;
            LobbyLabel2.Visible = false;
            LobbyLabel3.Visible = false;
            LobbyLabel4.Visible = false;
            LobbyPanel1.Visible = false;
            LobbyPanel2.Visible = false;
            LobbyPanel3.Visible = false;
            LobbyPanel4.Visible = false;

            Radiant1.Visible = false;
            Radiant2.Visible = false;
            Radiant3.Visible = false;
            Radiant4.Visible = false;

            Dire1.Visible = false;
            Dire2.Visible = false;
            Dire3.Visible = false;
            Dire4.Visible = false;

            LobbyTitle1.Visible = false;
            LobbyTitle2.Visible = false;
            LobbyTitle3.Visible = false;
            LobbyTitle4.Visible = false;


            JoinRadiant1.Visible = false;
            JoinRadiant2.Visible = false;
            JoinRadiant3.Visible = false;
            JoinRadiant4.Visible = false;

            JoinDire1.Visible = false;
            JoinDire2.Visible = false;
            JoinDire3.Visible = false;
            JoinDire4.Visible = false;


            RadiantSlot11.Visible = false;
            RadiantSlot12.Visible = false;
            RadiantSlot13.Visible = false;
            RadiantSlot14.Visible = false;
            RadiantSlot15.Visible = false;

            DireSlot11.Visible = false;
            DireSlot12.Visible = false;
            DireSlot13.Visible = false;
            DireSlot14.Visible = false;
            DireSlot15.Visible = false;


            RadiantSlot21.Visible = false;
            RadiantSlot22.Visible = false;
            RadiantSlot23.Visible = false;
            RadiantSlot24.Visible = false;
            RadiantSlot25.Visible = false;

            DireSlot21.Visible = false;
            DireSlot22.Visible = false;
            DireSlot23.Visible = false;
            DireSlot24.Visible = false;
            DireSlot25.Visible = false;


            RadiantSlot31.Visible = false;
            RadiantSlot32.Visible = false;
            RadiantSlot33.Visible = false;
            RadiantSlot34.Visible = false;
            RadiantSlot35.Visible = false;

            DireSlot31.Visible = false;
            DireSlot32.Visible = false;
            DireSlot33.Visible = false;
            DireSlot34.Visible = false;
            DireSlot35.Visible = false;


            RadiantSlot41.Visible = false;
            RadiantSlot42.Visible = false;
            RadiantSlot43.Visible = false;
            RadiantSlot44.Visible = false;
            RadiantSlot45.Visible = false;
            
            DireSlot41.Visible = false;
            DireSlot42.Visible = false;
            DireSlot43.Visible = false;
            DireSlot44.Visible = false;
            DireSlot45.Visible = false;

        }

        private void Form1_Load(object sender, EventArgs e) {

            //TODO: Start networking
            server = new ServerHandler(this);
            server.ConnectToServer();

            InitUI();


            bool badconnection = false;
            bool baduser = false;
            while (true) {
                LoginPortal login = new LoginPortal(this, badconnection, baduser);
                baduser = false;
                badconnection = false;

                DialogResult result = login.ShowDialog();

                if (result == DialogResult.Cancel) {
                    Application.Exit();
                    break;
                }
                if (result == DialogResult.OK) {
                    //exiting with success;
                    break;

                } else if (result == DialogResult.No) {
                    badconnection = true;
                } else if (result == DialogResult.Retry) {
                    baduser = true;
                }
                login.Dispose();
            } // wait for successful login



            InitIRC();
            TabManageThread = new Thread(() => {
                int TabIndex = -1;
                while(!closing) {
                    TabIndex = materialTabControl1.SelectedIndex;
                    //TODO: get tab update data & run callbacks (if connected to server)
                    Thread.Sleep(100);
                }
            });
        }
        private delegate void UpdatePlayerListCallback(string[] usernames);
        public void UpdatePlayerList(string[] usernames) {
            if(richTextBox2.InvokeRequired) {
                UpdatePlayerListCallback callback = new UpdatePlayerListCallback(UpdatePlayerList);
                richTextBox2.Invoke(callback, new object[] { usernames });
                return;
            }


            richTextBox2.Text = "";
            foreach (string name in usernames) {
                if(richTextBox2.Text != "") {
                    richTextBox2.Text += "\n";
                }
                richTextBox2.Text += name;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            server.DisconnectFromServer();
            closing = true;
        }

        private void button1_Click(object sender, EventArgs e) {
            server.TestConnection();
        }
        
        //change bot info
        public delegate void ChangeBotStatusCallback(int botIndex, bool isInUse);
        public void ChangeBotStatus(int botIndex, bool isInUse) {
            if(this.InvokeRequired) {
                ChangeBotStatusCallback callback = new ChangeBotStatusCallback(ChangeBotStatus);
                this.Invoke(callback, botIndex, isInUse);
                return;
            }

            BotDataReceived(); //make sure we register bot data as received

            if (isInUse) {
                TabUI.ShowGame(botIndex);
            } else {
                TabUI.ShowLobby(botIndex);
            }
        }


        // Loading  player list in lobbies/games ?
        public delegate void LoadPlayersCallback(int BotIndex, List<ulong> Radiant, List<ulong> Dire);
        public void LoadLobbyPlayers(int BotIndex, List<ulong> Radiant, List<ulong> Dire) {
            if (this.InvokeRequired) {
                LoadPlayersCallback callback = new LoadPlayersCallback(LoadLobbyPlayers);
                this.Invoke(callback,new object[] { BotIndex, Radiant, Dire });
                return;
            }
            Panel gamePanel = (Panel)this.tabPage2.Controls["LobbyPanel" + (BotIndex + 1).ToString()];
            if (gamePanel == null) {
                return;
            }

            List<MaterialLabel> radiantLabels = TabUI.FindChildControls(gamePanel, "RadiantSlot").Cast<MaterialLabel>().ToList();
            List<MaterialLabel> direLabels = TabUI.FindChildControls(gamePanel, "DireSlot").Cast<MaterialLabel>().ToList();

            foreach (MaterialLabel label in radiantLabels) {
                label.Text = "Unassigned";
            }
            foreach (MaterialLabel label in direLabels) {
                label.Text = "Unassigned";
            }
            int i = 0;
            foreach (ulong steamid in Radiant) {
                string name = server.OnlineUsers[steamid];
                if (name == null) {
                    name = "Error: Unknown";
                }
                radiantLabels[i].Text = name;
                i++;
            }
            i = 0;
            foreach (ulong steamid in Dire) {
                string name = server.OnlineUsers[steamid];
                if (name == null) {
                    name = "Error: Unknown";
                }
                direLabels[i].Text = name;
                i++;
            }
        }

        public void LoadGamePlayers(int BotIndex, List<ulong> Radiant, List<ulong> Dire) {
            if (this.InvokeRequired) {
                LoadPlayersCallback callback = new LoadPlayersCallback(LoadGamePlayers);
                this.Invoke(callback, new object[] { BotIndex, Radiant, Dire });
                return;
            }

            Panel gamePanel = (Panel)this.tabPage3.Controls["GamePanel" + (BotIndex + 1).ToString()];
            if(gamePanel == null) {
                return;
            }

            List<MaterialLabel> radiantLabels = TabUI.FindChildControls(gamePanel, "RadiantPlayer").Cast<MaterialLabel>().ToList();
            List<MaterialLabel>  direLabels = TabUI.FindChildControls(gamePanel, "DirePlayer").Cast<MaterialLabel>().ToList();

            foreach (MaterialLabel label in radiantLabels) {
                label.Text = "Unassigned";
            }
            foreach (MaterialLabel label in direLabels) {
                label.Text = "Unassigned";
            }
            int i = 0;
            foreach (ulong steamid in Radiant) {
                string name = server.OnlineUsers[steamid];
                if (name == null) {
                    name = "Error: Unknown";
                }
                radiantLabels[i].Text = name;
                i++;
            }
            i = 0;
            foreach (ulong steamid in Dire) {
                string name = server.OnlineUsers[steamid];
                if (name == null) {
                    name = "Error: Unknown";
                }
                direLabels[i].Text = name;
                i++;
            }
        }

        public delegate void ShowMsgCallback(string msg);
        public void ShowMsg(string msg) {
            if(this.InvokeRequired) {
                ShowMsgCallback callback = new ShowMsgCallback(ShowMsg);
                this.Invoke(callback, msg);
                return;
            }
            Notification.Show("Notification", msg);
        }

        //Update information on a specific bot
        // This is called on first load and on bot status change
        // This is called when 
        //    Players Leave
        //    Players Join
        //    Game Started
        //    Game Over
        public void UpdateBotInfo(int BotIndex, bool isInUse,List<ulong> RadiantSteamIDs, List<ulong> DireSteamIDs) {
            ChangeBotStatus(BotIndex, isInUse); //hide/show bot in lobby or in game
            if(isInUse) {
                LoadGamePlayers(BotIndex, RadiantSteamIDs, DireSteamIDs); // load game members
            } else {
                LoadLobbyPlayers(BotIndex, RadiantSteamIDs, DireSteamIDs); // load lobby members
            }
        }

        public delegate void ResetLeaveButtonCallback(int boIndex);
        public void ResetLeaveButton(int botIndex) {
            if(this.InvokeRequired) {
                ResetLeaveButtonCallback callback = new ResetLeaveButtonCallback(ResetLeaveButton);
                this.Invoke(callback, botIndex);
                return;
            }

            if(botIndex == 0) {
                JoinRadiant1.Click -= JoinRadiant1_Click;
                JoinRadiant1.Click -= LeaveRadiant1_Click;
                JoinRadiant1.Click += JoinRadiant1_Click;

                JoinRadiant1.Text = "Join";

                JoinDire1.Click -= JoinDire1_Click;
                JoinDire1.Click -= LeaveDire1_Click;
                JoinDire1.Click += JoinDire1_Click;

                JoinDire1.Text = "Join";
            } else if(botIndex == 1) {
                JoinRadiant2.Click -= JoinRadiant2_Click;
                JoinRadiant2.Click -= LeaveRadiant2_Click;
                JoinRadiant2.Click += JoinRadiant2_Click;

                JoinRadiant2.Text = "Join";

                JoinDire2.Click -= JoinDire2_Click;
                JoinDire2.Click -= LeaveDire2_Click;
                JoinDire2.Click += JoinDire2_Click;

                JoinDire2.Text = "Join";
            } else if(botIndex == 2) {
                JoinRadiant3.Click -= JoinRadiant3_Click;
                JoinRadiant3.Click -= LeaveRadiant3_Click;
                JoinRadiant3.Click += JoinRadiant3_Click;

                JoinRadiant3.Text = "Join";

                JoinDire3.Click -= JoinDire3_Click;
                JoinDire3.Click -= LeaveDire3_Click;
                JoinDire3.Click += JoinDire3_Click;

                JoinDire1.Text = "Join";
            } else {
                JoinRadiant4.Click -= JoinRadiant4_Click;
                JoinRadiant4.Click -= LeaveRadiant4_Click;
                JoinRadiant4.Click += JoinRadiant4_Click;

                JoinRadiant4.Text = "Join";

                JoinDire4.Click -= JoinDire4_Click;
                JoinDire4.Click -= LeaveDire4_Click;
                JoinDire4.Click += JoinDire4_Click;

                JoinDire4.Text = "Join";
            }
        }

        private void AttemptLeaveLobby(int botIndex, int team) {
            if (!server.LeaveLobby(botIndex, team)) {
                
            } else {
                switch (botIndex) {
                    case 0: {
                            if (team == 0) {
                                JoinRadiant1.Click += JoinRadiant1_Click;
                                JoinRadiant1.Click -= LeaveRadiant1_Click;

                                JoinRadiant1.Text = "Join";
                            } else {
                                JoinDire1.Click += JoinDire1_Click;
                                JoinDire1.Click -= LeaveDire1_Click;

                                JoinDire1.Text = "Join";
                            }
                            break;
                        }
                    case 1: {
                            if (team == 0) {
                                JoinRadiant2.Click += JoinRadiant2_Click;
                                JoinRadiant2.Click -= LeaveRadiant2_Click;

                                JoinRadiant2.Text = "Join";
                            } else {
                                JoinDire2.Click += JoinDire2_Click;
                                JoinDire2.Click -= LeaveDire2_Click;

                                JoinDire2.Text = "Join";
                            }
                            break;
                        }
                    case 2: {
                            if (team == 0) {
                                JoinRadiant3.Click += JoinRadiant3_Click;
                                JoinRadiant3.Click -= LeaveRadiant3_Click;

                                JoinRadiant3.Text = "Join";
                            } else {
                                JoinDire3.Click += JoinDire3_Click;
                                JoinDire3.Click -= LeaveDire3_Click;

                                JoinDire3.Text = "Join";
                            }
                            break;
                        }
                    case 3: {
                            if (team == 0) {
                                JoinRadiant4.Click += JoinRadiant4_Click;
                                JoinRadiant4.Click -= LeaveRadiant4_Click;

                                JoinRadiant4.Text = "Join";
                            } else {
                                JoinDire4.Click += JoinDire4_Click;
                                JoinDire4.Click -= LeaveDire4_Click;

                                JoinDire4.Text = "Join";
                            }
                            break;
                        }
                }
            }
        }
        private void AttemptJoinLobby(int botIndex, int team) {
            if (!server.JoinLobby(botIndex, team)) {
                
            } else {
                switch(botIndex) {
                    case 0: {
                            if(team == 0) {
                                JoinRadiant1.Click -= JoinRadiant1_Click;
                                JoinRadiant1.Click += LeaveRadiant1_Click;

                                JoinRadiant1.Text = "Leave";
                            } else {
                                JoinDire1.Click -= JoinDire1_Click;
                                JoinDire1.Click += LeaveDire1_Click;

                                JoinDire1.Text = "Leave";
                            }
                            break;
                        }
                    case 1: {
                            if (team == 0) {
                                JoinRadiant2.Click -= JoinRadiant2_Click;
                                JoinRadiant2.Click += LeaveRadiant2_Click;

                                JoinRadiant2.Text = "Leave";
                            } else {
                                JoinDire2.Click -= JoinDire2_Click;
                                JoinDire2.Click += LeaveDire2_Click;

                                JoinDire2.Text = "Leave";
                            }
                            break;
                        }
                    case 2: {
                            if (team == 0) {
                                JoinRadiant3.Click -= JoinRadiant3_Click;
                                JoinRadiant3.Click += LeaveRadiant3_Click;

                                JoinRadiant3.Text = "Leave";
                            } else {
                                JoinDire3.Click -= JoinDire3_Click;
                                JoinDire3.Click += LeaveDire3_Click;

                                JoinDire3.Text = "Leave";
                            }
                            break;
                        }
                    case 3: {
                            if (team == 0) {
                                JoinRadiant4.Click -= JoinRadiant4_Click;
                                JoinRadiant4.Click += LeaveRadiant4_Click;

                                JoinRadiant4.Text = "Leave";
                            } else {
                                JoinDire4.Click -= JoinDire4_Click;
                                JoinDire4.Click += LeaveDire4_Click;

                                JoinDire4.Text = "Leave";
                            }
                            break;
                        }
                }
            }
        }

        private void LeaveRadiant1_Click(object sender, EventArgs e) {
            AttemptLeaveLobby(0, 0);
        }

        private void LeaveDire1_Click(object sender, EventArgs e) {
            AttemptLeaveLobby(0, 1);
        }

        private void LeaveRadiant2_Click(object sender, EventArgs e) {
            AttemptLeaveLobby(1, 0);
        }

        private void LeaveDire2_Click(object sender, EventArgs e) {
            AttemptLeaveLobby(1, 1);
        }

        private void LeaveRadiant3_Click(object sender, EventArgs e) {
            AttemptLeaveLobby(2, 0);
        }

        private void LeaveDire3_Click(object sender, EventArgs e) {
            AttemptLeaveLobby(2, 1);
        }

        private void LeaveRadiant4_Click(object sender, EventArgs e) {
            AttemptLeaveLobby(3, 0);
        }

        private void LeaveDire4_Click(object sender, EventArgs e) {
            AttemptLeaveLobby(3, 1);
        }


        private void JoinRadiant1_Click(object sender, EventArgs e) {
            AttemptJoinLobby(0, 0);
        }

        private void JoinDire1_Click(object sender, EventArgs e) {
            AttemptJoinLobby(0, 1);
        }

        private void JoinRadiant2_Click(object sender, EventArgs e) {
            AttemptJoinLobby(1, 0);
        }

        private void JoinDire2_Click(object sender, EventArgs e) {
            AttemptJoinLobby(1, 1);
        }

        private void JoinRadiant3_Click(object sender, EventArgs e) {
            AttemptJoinLobby(2, 0);
        }

        private void JoinDire3_Click(object sender, EventArgs e) {
            AttemptJoinLobby(2, 1);
        }

        private void JoinRadiant4_Click(object sender, EventArgs e) {
            AttemptJoinLobby(3, 0);
        }

        private void JoinDire4_Click(object sender, EventArgs e) {
            AttemptJoinLobby(3, 1);
        }
    }
}
