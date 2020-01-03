using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace Cy_Connection_Client
{
    public class Client_module : DataQueue
    {
        public delegate void MessingDelegate(string sender, object obj, int RoomId);
        public delegate void FileMessDelegate(string sender, int indexoffile, string filename, int roomid);
        public delegate void FileDownloadDelegate(byte[] file,int roomId);
        public delegate void LoginDelegate(string username);
        public delegate void LogoutDelegate(string username, int room);
        public delegate void ListClientDelegate(string[] listClients);
        public delegate void CreatRoomDelegate(int id, string[] listMember);
        
        /// <summary>
        /// Sự kiện xảy ra khi nhận tin nhắn là text ( người gửi, nọi dung , mã phòng )
        /// </summary>
        public event MessingDelegate ReciveTextEvent;
        /// <summary>
        /// Sự kiện xảy ra khi nhận tin nhắn hình  ( người gửi, nọi dung , mã phòng )
        /// </summary>
        public event MessingDelegate ReciveImageEvent;
        /// <summary>
        /// Sự kiện xảy ra khi 1 ngườ dùng khác gửi tin nhắn đến phòng (người gửi, mã file, tên file, mã phòng )
        /// </summary>
        public event FileMessDelegate ReciveFileMessEvent;
        /// <summary>
        /// Sự kiện xảy ra khi người dung download thành công 1 file
        /// </summary>
        public event FileDownloadDelegate ReceiveFileEvent;
        /// <summary>
        /// Sự kiện xảy ra khi client Login và Sever gửi về kết quả nếu login thành công ( dùng để xác định Login dc hay o )
        /// </summary>
        public event LoginDelegate ReciveLoginEvent;
        /// <summary>
        /// Sự kiệnxảy ra khi dđối phương trong phòng chat đã logout
        /// </summary>
        public event LogoutDelegate ReceiveLogoutEvent;
        /// <summary>
        /// Sự kiện xảy ra khi sever đưa cho client 1 list danh sách các user đang online
        /// </summary>
        public event ListClientDelegate ReceiveListClientEvent;
        /// <summary>
        /// Sự kiện xả ra khi client nhận dc yêu cầu vào 1 phòng chat
        /// </summary>
        public event CreatRoomDelegate ReceiveCreatRoomEvent;
        protected IPEndPoint Ip;
        protected Socket Client;



        public void Connect()
        {

            // Còn thiếu 2 công đoạn là size định size data cho mỗi lần gửi và xác định IP phú hợp ,nên tạm thời set tĩnh
            sizeofdata = 20000;
            Ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                Client.Connect(Ip);
            }
            catch (Exception e)
            {
                Console.Write("Error in cpnnect " + e);
            }
            Thread RecieveThread = new Thread(Receiver);
            RecieveThread.IsBackground = true;
            RecieveThread.Start();

        }

        public void Disconnect()
        {
            Client.Close();
        }

        public void Send(object obj, DataType type, byte RoomId)
        {
            PhanManh divide = new PhanManh(sizeofdata, (byte)new Random().Next(1, 254), Client, type, RoomId);
            if (type == DataType.Image)
            {
                divide.DivideAndSend(DataConverter.Serialize_Image(obj));
            }
            if (type == DataType.Text || type == DataType.Login|| type == DataType.Logout || type == DataType.CreatRoom || type == DataType.Filename || type == DataType.DownloadFile)
            {
                divide.DivideAndSend(DataConverter.Serialize_Text(obj));
            }
            if (type == DataType.File)
            {
                divide.DivideAndSend(DataConverter.Serialize_File(obj.ToString()));
            }
        }

        /// <summary>
        /// Gửi tin nhắn chữ
        /// </summary>
        /// <param name="mess">tin nhắn</param>
        /// <param name="roomid">mã phòng</param>
        public void SendText(string mess, byte roomid)
        {
            Send(mess, DataType.Text, roomid);
        }


        /// <summary>
        /// Gửi ảnh 
        /// </summary>
        /// <param name="image">ảnh</param>
        /// <param name="roomid">mã phòng</param>
        public void SendImage(Image image, byte roomid)
        {
            Send(image, DataType.Image, roomid);
        }

        /// <summary>
        /// Gửi file 
        /// vd : gửi file tại E:\folder\xxx.txt
        /// path = "E:\\folder"
        /// filename = "xxx.txt"
        /// </summary>
        /// <param name="path">Đường dẫn đến file</param>
        /// <param name="filename">tên file</param>
        /// <param name="roomid">mã phòng</param>
        public void SendFile(string path, string filename, byte roomid)
        {
            string full = path + "\\" + filename;
            Send(filename, DataType.Filename, roomid);

            Send(full, DataType.File, roomid);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="pass">password</param>
        public void LogIn(string username, string pass)
        {
            string user_pass = username + " " + pass;
            Send(user_pass, DataType.Login, 0);
        }
        
        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <param name="username"></param>
        public void Logout(string username)
        {
            Send(username, DataType.Logout, 0);
        }

        /// <summary>
        /// Tạo 1 phòng chat
        /// </summary>
        /// <param name="listmember">danh sách các username muốn chat cùng</param>
        public void CreatRoomChat(List<string> listmember)
        {
            string usernames = "";
            foreach (string name in listmember)
            {
                usernames += name + " ";
            }
            Send(usernames, DataType.CreatRoom, 0);
        }
    
        /// <summary>
        /// Gửi yêu cầu tải file có mã là index
        /// </summary>
        /// <param name="index">mã file cần tải</param>
        public void RequestDownloadFile(int index,int RoomId)
        {
            Send(index.ToString(), DataType.DownloadFile, (byte)RoomId);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// chờ nhận tin nhắn
        /// </summary>
        private void Receiver()
        {
            try
            {
                while (true)
                {
                    byte[] tempdata = new byte[sizeofdata + 2];
                    Client.Receive(tempdata);
                    Extract_n_Pull(tempdata);

                }
            }
            catch (Exception e)
            {
                Console.Write("Client reciver err " + e);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        //tên thằng vừa gửi tin nhắn
        protected string SenderName = "";


        protected override void We_Have_Data_here(Socket sender, byte[] data, byte index, DataType type, byte RoomId)
        {
            if (type == DataType.Text)
            {
                object mess = DataConverter.Deserialize_Text(data);
                if (ReciveTextEvent != null)
                {
                    ReciveTextEvent(SenderName, mess, RoomId);
                }
            }
            if (type == DataType.Image)
            {
                object img = DataConverter.DeSerialize_Image(data);
                if (ReciveImageEvent != null)
                {
                    ReciveImageEvent(SenderName, img, RoomId);
                }
            }
            if (type == DataType.Login)
            {
                object LogInresult = DataConverter.Deserialize_Text(data);
                if (ReciveLoginEvent != null)
                {
                    ReciveLoginEvent((string)LogInresult);
                }
            }
            if (type == DataType.Logout)
            {
                object LogoutMess = DataConverter.Deserialize_Text(data);
                if (ReceiveLogoutEvent != null)
                {
                    ReceiveLogoutEvent((string)LogoutMess, RoomId);
                }
            }

            if (type == DataType.ListClient)
            {
                object ClientsResult = DataConverter.Deserialize_Text(data);
                string result = (string)ClientsResult;
                string[] split = new string[] { " " };
                String[] finalresult = result.Split(split, StringSplitOptions.RemoveEmptyEntries);
                ReceiveListClientEvent(finalresult);
            }
            if (type == DataType.CreatRoom)
            {
                object members = DataConverter.Deserialize_Text(data);
                string result = (string)members;
                string[] split = new string[] { " " };
                String[] finalresult = result.Split(split, StringSplitOptions.RemoveEmptyEntries);
                ReceiveCreatRoomEvent(RoomId, finalresult);
            }

            if (type == DataType.SenderUsername)
            {
                SenderName = (string)DataConverter.Deserialize_Text(data);
            }

            if (type == DataType.File)
            {
                object mess = DataConverter.Deserialize_Text(data);
                string info = (string)mess;
                string[] splistr = { "@@@" };
                int indexoffile = int.Parse(info.Split(splistr, StringSplitOptions.RemoveEmptyEntries)[0]);
                string filename = info.Split(splistr, StringSplitOptions.RemoveEmptyEntries)[1];
                ReciveFileMessEvent(SenderName, indexoffile, filename, RoomId);
            }
            if (type == DataType.DownloadFile)
            {
                ReceiveFileEvent(data,RoomId);
            }

        }

    }

}