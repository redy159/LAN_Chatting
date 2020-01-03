using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cy_Connection_Client;
using Clients.WinForms;
using System.Threading;

namespace Clients
{
    public partial class GroupList : Form
    {
        Client_module myClient;
        List<string> onlineUser = new List<string>();
        List<ChatBox> listChatBox = new List<ChatBox>();
        OnlineClientItem onlineClientItem;
        Login loginform;
        string username = "";
        public GroupList()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            CreateConnection();
            Login();
        }

        private void CreateConnection()
        {
            myClient = new Client_module();
            myClient.Connect();
            myClient.ReceiveCreatRoomEvent += MyClient_ReceiveCreatRoomEvent;
            myClient.ReceiveListClientEvent += MyClient_ReceiveListClientEvent1;
            myClient.ReceiveLogoutEvent += MyClient_ReceiveLogoutEvent;
            myClient.ReciveFileMessEvent += MyClient_ReciveFileMessEvent;
            myClient.ReceiveFileEvent += MyClient_ReceiveFileEvent1;
            myClient.ReciveImageEvent += MyClient_ReciveImageEvent;
            myClient.ReciveLoginEvent += MyClient_ReciveLoginEvent;
            myClient.ReciveTextEvent += MyClient_ReciveTextEvent;
        }
        private void Login()
        {
            this.Hide();
            loginform = new Login();
            loginform.GroupListReference(this);
            DialogResult result = loginform.ShowDialog();
            if (result == DialogResult.OK)
            {
                username = loginform.username;
                if (String.IsNullOrEmpty(username) || String.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Tên đăng nhập không được rỗng !!!");
                    Login();
                }
                else
                {
                    this.Visible = true;
                    myClient.LogIn(username, "WeDontHavePass");
                }
            }
            else this.Close();
        }

        void InitializeListClient()
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    buttonFlowLayoutPanel.Controls.Clear();
                });
            }
            else
            {
                buttonFlowLayoutPanel.Controls.Clear();
            }

            foreach (string name in onlineUser)
            {
                OnlineClientItem item = new OnlineClientItem(name);
                item.CreatRoomButtonEvenClick += Item_CreatRoomButtonEvenClick;
                onlineClientItem = item;
                if (name == username)
                {
                    item.setCurrentUser();
                }
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        buttonFlowLayoutPanel.Controls.Add(item);
                    });
                }
                else
                {
                    buttonFlowLayoutPanel.Controls.Add(item);
                }
            }
        }
        bool CheckListClient(string _userName)
        {
            if (onlineUser != null)
            {
                foreach (string onlineUserName in onlineUser)
                {
                    if (_userName == onlineUserName) return true;
                }
            }
            return false;
        }
        private void Item_CreatRoomButtonEvenClick(string _username)
        {
            if (this.username == _username)
            {
                MessageBox.Show("You cannot create your own room");
            }
            else
            {
                foreach (ChatBox cb in listChatBox)
                {
                    if ((username == cb.Member[0] && _username == cb.Member[1]) || ((username == cb.Member[1] && _username == cb.Member[0])))
                    {
                        new Thread(() => cb.ShowDialog()).Start();
                        return;
                    }
                }

                List<string> room = new List<string>();
                room.Add(this.username);
                room.Add(_username);
                myClient.CreatRoomChat(room);
            }
        }
        private void MyClient_ReciveTextEvent(string sender, object obj, int RoomId)
        {
            if (sender == username)
            {
                return;
            }

            foreach (ChatBox cb in listChatBox)
            {
                if (cb.RoomId == RoomId)
                {
                    cb.ReceiveMessText(sender, obj);
                }
            }
        }
        private void MyClient_ReciveLoginEvent(string username)
        {
            if (username != "UserAlreadyLogin" && username != "UserEmpty")
            {
                this.Visible = true;
                this.Text = "Username : " + username;
                MessageBox.Show("Đăng nhập thành công với Username " + username);
                this.username = username;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tên đăng nhập khác");
                Login();
            }
        }
        private void MyClient_ReciveImageEvent(string sender, object obj, int RoomId)
        {
            if (sender == username)
            {
                return;
            }
            foreach (ChatBox cb in listChatBox)
            {
                if (cb.RoomId == RoomId)
                {
                    cb.ReceiveImage(sender, obj);
                }
            }
        }
        private void MyClient_ReciveFileMessEvent(string sender, int indexoffile, string filename, int roomid)
        {
            if (sender == username)
            {
                return;
            }
            foreach (ChatBox cb in listChatBox)
            {
                if (cb.RoomId == roomid)
                {
                    cb.ReceiveFileMess(sender, indexoffile, filename, roomid);
                }
            }
        }
        private void MyClient_ReceiveFileEvent1(byte[] file, int roomId)
        {

            foreach (ChatBox cb in listChatBox)
            {
                if (cb.RoomId == roomId)
                {
                    cb.ReceiveFileDownLoad(file);
                }
            }
        }
        private void MyClient_ReceiveLogoutEvent(string username, int roomid)
        {
            ChatBox cb = listChatBox.Find(i => i.RoomId == roomid);
            cb.PartnerLogout(username);
            cb.Close();
            listChatBox.Remove(cb);
            //foreach (ChatBox cb in listChatBox)
            //{
            //    if (cb.RoomId == roomid)
            //    {
            //        cb.PartnerLogout(username);
            //        cb.Close();
            //    }
            //}
        }
        private void MyClient_ReceiveListClientEvent1(string[] listClients)
        {
            //xóa control có sẵn

            onlineUser = new List<string>(listClients);
            InitializeListClient();
        }
        private void MyClient_ReceiveCreatRoomEvent(int id, string[] listMember)
        {
            string title;
            if (listMember[0] == username)
            {
                title = "Username : " + username + " chatting with " + listMember[1];
            }
            else
            {
                title = "Username : " + username + " chatting with " + listMember[0];
            }
            ChatBox box = new ChatBox(id, title, listMember);
            box.RoomSendFileEvent += Box_RoomSendFileEvent;
            box.RoomSendImageEvent += Box_RoomSendImageEvent;
            box.RoomSendMessEvent += Box_RoomSendMessEvent;
            box.RoomRequestDownload += Box_RoomRequestDownload;
            listChatBox.Add(box);

            /// tạo 1 luồng mới để form chạy
            new Thread(() => box.ShowDialog()).Start();

        }
        private void Box_RoomRequestOpenBrowserDialog(byte[] data, int roomid)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (ChatBox cb in listChatBox)
                {
                    if (cb.RoomId == roomid)
                    {
                        cb.ReciverSavepath(dialog.SelectedPath, data);
                    }
                }
            }
        }
        private void Box_RoomRequestDownload(int id, int roomid)
        {
            myClient.RequestDownloadFile(id, roomid);
        }

        #region xu li event cho 1 room
        private void Box_RoomSendMessEvent(object obj, int RoomId)
        {
            myClient.SendText(obj.ToString(), (byte)RoomId);
        }
        private void Box_RoomSendImageEvent(object obj, int RoomId)
        {
            myClient.SendImage(obj as Image, (byte)RoomId);
        }
        private void Box_RoomSendFileEvent(string path, string filename, int roomid)
        {
            myClient.SendFile(path, filename, (byte)roomid);
        }
        #endregion

        private void GroupList_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void linkLabel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            myClient.Logout(this.username);
            listChatBox.Clear();
            this.Hide();
            Login();
        }
    }
}
