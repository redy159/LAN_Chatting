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
using System.Drawing.Imaging;

namespace Cy_Connection_Sever
{

    struct Client
    {
        public Socket socket;
        public string username;
    }

    class Room
    {
        public byte Id;
        public List<Client> Members;
        public Room() { Members = new List<Client>(); }
    }

    public class Sever_module : DataQueue
    {
        public delegate void MessingDelegate(object obj, int RoomId);
        private IPEndPoint Ip;
        private Socket Sever;
        private List<Socket> tempclient;
        private List<Client> ListClients;
        private List<Room> ListRoom;
        /// //////////////////////////////////////////////////////////////////////////////

        #region kết nối
        public void Connect()
        {
            sizeofdata = 20000;
            ListClients = new List<Client>();
            tempclient = new List<Socket>();
            ListRoom = new List<Room>();

            Ip = new IPEndPoint(IPAddress.Any, 9999);
            Sever = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            Sever.Bind(Ip);
            Thread Listen_new_cline = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Sever.Listen(100);
                        Socket client = Sever.Accept();
                        tempclient.Add(client);
                        Thread Receive = new Thread(Receiver);
                        Receive.IsBackground = true;
                        Receive.Start(client);

                    }

                }
                catch (Exception e)
                {
                    Console.Write("Reset sever network");
                    Ip = new IPEndPoint(IPAddress.Any, 9999);
                    Sever = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });

            Listen_new_cline.IsBackground = true;
            Listen_new_cline.Start();
        }

        public void Disconnect()
        {
            Sever.Close();
        }

        #endregion

        #region Gửi
        /// <summary>
        /// Gửi bình thường ( gửi theo mã phòng )
        /// </summary>
        /// <param name="obj">nội dung gửi</param>
        /// <param name="type">enum loại thông tin cần gửi</param>
        /// <param name="roomid">mã phòng</param>
        public void Send(object obj, DataType type, byte roomid)
        {
            //xác định phòng
            Room room = new Room();
            foreach (Room _room in ListRoom)
            {
                if (_room.Id == roomid)
                {
                    room = _room;
                    break;
                }
            }
            // gửi nội dung éến con dân trong phòng
            foreach (Client member in room.Members)
            {
                Send_1_Client(obj, (byte)new Random().Next(1, 244), type, member.socket, room.Id);
            }
        }

        /// <summary>
        /// Gửi đến toàn thể dân đen đã login
        /// </summary>
        /// <param name="obj">nội dung gửi</param>
        /// <param name="type">enum loại thông tin cần gửi</param>
        public void SendAll(object obj, DataType type)
        {
            foreach (Client _client in ListClients)
            {

                Send_1_Client(obj, (byte)new Random().Next(1, 244), type, _client.socket, 0);

            }
        }

        /// <summary>
        /// Gửi đến 1 thằng lờ nào đó 
        /// </summary>
        /// <param name="obj">nội dung tin nhắn gừi</param>
        /// <param name="type">enum loại thông tin cần gửi</param>
        /// <param name="socket">Socket của thằng lờ cần gửi</param>
        /// <param name="roomid">và dĩ nhiên cũng cần mã phòng </param>
        public void Send_1_Client(object obj, byte index, DataType type, Socket socket, byte roomid)
        {
           //lưu ý ở đây check thêm trường hợp client được gửi tuy còn trong phòng nhưng không còn on nữa ( lỗi socket )
           //hiện tại chỉ xét chat 2 nguồi nên lỗi này cho qua , lưu ý kho phát triển lên chat nhóm
            PhanManh Divide = new PhanManh(sizeofdata, index, socket, type, roomid);
            if (type == DataType.Image)
            {
                Divide.DivideAndSend(DataConverter.Serialize_Image(obj));
            }
            if (type == DataType.Text || type == DataType.Login || type== DataType.Logout || type == DataType.ListClient || type == DataType.CreatRoom || type == DataType.SenderUsername || type == DataType.File)
            {
                Divide.DivideAndSend(DataConverter.Serialize_Text(obj));
            }

        }
        /// <summary>
        /// Gửi đến 1 thằng lờ nào đó 
        /// </summary>
        /// <param name="obj">nội dung tin nhắn gừi</param>
        /// <param name="type">enum loại thông tin cần gửi</param>
        /// <param name="socket">Socket của thằng lờ cần gửi</param>
        /// <param name="roomid">và dĩ nhiên cũng cần mã phòng </param>
        public void Send_1_Client(byte[] data, byte index, DataType type, Socket socket, byte roomid)
        {

            PhanManh Divide = new PhanManh(sizeofdata, index, socket, type, roomid);
           
                Divide.DivideAndSend(data);
            

        }

        /// <summary>
        /// Đưa danh sách các username cho các client đã login
        /// </summary>
        public void Ping()
        {
            string usernameStr = "";
            foreach (Client client in ListClients)
            {
                usernameStr = usernameStr + " " + client.username;
            }

            SendAll(usernameStr, DataType.ListClient);

        }
        #endregion

        #region Nhận
        private void Receiver(object mclient)
        {
            Console.Write("client connected");
            Socket client = mclient as Socket;
            try
            {
                byte[] data;
                while (true)
                {
                    data = new byte[sizeofdata + 2];
                    client.Receive(data);
                    Extract_n_Pull(client, data);
                }
            }
            catch (Exception e)
            {

                foreach (Client _client in ListClients)
                {
                    if (_client.socket == client)
                    {
                        ListClients.Remove(_client);
                        break;
                    }
                }
                client.Close();
            }
        }


        /// <summary>
        /// Gửi tin nhắn đến 1 room cụ thể
        /// </summary>
        /// <param name="sender">thằng gửi</param>
        /// <param name="mess">Tin nhắn</param>
        /// <param name="type">enum loại thông tin cần gửi</param>
        /// <param name="Roomid">Mã phòng</param>
        public void SendMesstoRoomm(Socket sender, object mess, DataType type, int Roomid)
        {
            Room room = new Room();
            //xác định phònh cần gửi
            foreach (Room r in ListRoom)
            {
                if (r.Id == Roomid)
                {
                    room = r;
                    break;
                }
            }

            //xác định thằng lờ client nào vừ gửi
            string sendername = "";
            foreach (Client cli in room.Members)
            {
                if (cli.socket == sender)
                {
                    sendername = cli.username;
                    break;
                }
            }
            //Gửi đến từng thằng client trong phòng. Bao gồm 2 tác vụ :
            // + Gửi tên thằng gửi
            // + Gửi nội dung
            byte index = (byte)new Random().Next(1, 244);
            foreach (Client client in room.Members)
            {
                Send_1_Client(sendername, index, DataType.SenderUsername, client.socket, room.Id);
            }
            Thread.Sleep(200);
            foreach (Client client in room.Members)
            {
                Send_1_Client(mess, index, type, client.socket, room.Id);
            }
        }


        /// <summary>
        /// Hàm phân loại thông tin khi nhận
        /// </summary>
        /// <param name="sender">thằng gửi</param>
        /// <param name="data">Tin nhắn</param>
        /// <param name="type">enum loại thông tin cần gửi</param>
        /// <param name="RoomId">mã phòng</param>
        protected override void We_Have_Data_here(Socket sender, byte[] data, byte index, DataType type, byte RoomId)
        {
            #region Text
            if (type == DataType.Text)
            {
                String mess = (string)DataConverter.Deserialize_Text(data);
          
                    SendMesstoRoomm(sender, mess, type, RoomId);
                
            }
            #endregion

            #region Image
            if (type == DataType.Image)
            {
                Image img = (Image)DataConverter.DeSerialize_Image(data);
               
                    //ReciveImageEvent(img, RoomId);
                    SendMesstoRoomm(sender, img, type, RoomId);
                
            }
            #endregion

            #region Login
            ///chỉnh sửa lại là string
            if (type == DataType.Login)
            {
                string username_pass = (string)DataConverter.Deserialize_Text(data);
                string[] space = new string[] { " " };
                string[] info = username_pass.Split(space, StringSplitOptions.RemoveEmptyEntries);
                string username = info[0];
                string pass = info[1];
                // Coi như là auto login

                tempclient.Remove(sender);

                Client client;
                client.socket = sender;
                client.username = username;
               
                foreach(Client cli in ListClients)
                {
                    if (cli.username == username)
                    {
                        Send_1_Client("UserAlreadyLogin", (byte)new Random().Next(1, 244), DataType.Login, sender, 0);
                        return;
                    }
                    if (cli.username == "")
                    {
                        Send_1_Client("UserEmpty", (byte)new Random().Next(1, 244), DataType.Login, sender, 0);
                        return;
                    }
                }
                ListClients.Add(client);
                Send_1_Client(username, (byte)new Random().Next(1, 244), DataType.Login, sender, 0);

            }
            #endregion

            #region Logout
            if (type == DataType.Logout)
            {
                string logoutusername = (string)DataConverter.Deserialize_Text(data);
                List<Room> Logoutroom = new List<Room>();
                //xác định những phòng bị logout
                foreach (Room room in ListRoom)
                {
                    foreach (Client mem in room.Members)
                    {
                        if (mem.socket == sender)
                        {
                            Logoutroom.Add(room);
                            break;
                        }
                    }
                }
                //gửi thông báo cho những username khác có trong phhòng
                foreach (Room room in Logoutroom)
                {
                    foreach (Client mem in room.Members)
                    {
                        if (mem.socket != sender)
                        {
                            Send_1_Client(logoutusername, (byte)new Random().Next(1, 244), DataType.Logout, mem.socket, room.Id);
                        }
                    }
                    ListRoom.Remove(room);
                }
                //Đưa lại hàng tạm
                tempclient.Add(sender);
                //xóa khỏi danh sách client đang on ( dĩ nhiên là gộp cái for này lên trên dc nhưng để riêng ra cho dễ hình dung )
                foreach (Client _client in ListClients)
                {
                    if (_client.socket == sender)
                    {
                        ListClients.Remove(_client);
                        break;
                    }
                }
            }
            #endregion

            #region tạo phòng
            if (type == DataType.CreatRoom)
            {
                byte id = (byte)(ListRoom.Count + 1);
                Room room = new Room();
                room.Id = id;

                string username = (string)DataConverter.Deserialize_Text(data);
                string[] split = new string[] { " " };
                string[] info = username.Split(split, StringSplitOptions.RemoveEmptyEntries);
                string menbers = " ";
                foreach (string name in info)
                {
                    foreach (Client client in ListClients)
                    {
                        if (name == client.username)
                        {
                            room.Members.Add(client);
                            break;
                        }
                    }
                    menbers += name + " ";
                }

                ListRoom.Add(room);
                Send(menbers, DataType.CreatRoom, room.Id);

            }
            #endregion

            #region File
            if (type == DataType.Filename)
            {
                FileControler.lastname = (string)DataConverter.Deserialize_Text(data);
            }
            if (type == DataType.File)
            {
                int key = FileControler.Savefile(data);
                string mess = key + "@@@" + FileControler.lastname;
                SendMesstoRoomm(sender, mess, type, RoomId);
            }

            #endregion

            #region downloadFIle
            if (type == DataType.DownloadFile)
            {
                String indexRequest = (string)DataConverter.Deserialize_Text(data);
                byte[] file = FileControler.loadfile(indexRequest);
                Send_1_Client(file, (byte)new Random().Next(1, 244), type, sender, RoomId);
            }
            #endregion

        }



        #endregion




    }

}

