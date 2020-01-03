using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;


namespace Cy_Connection_Client

{
    public enum DataType
    {
        Text = 0,
        Image = 1,
        File = 2,
        DownloadFile = 3,
        Login = 10,
        Logout = 11,
        ListClient = 12,
        CreatRoom = 13,
        SenderUsername = 14,
        Filename = 15
    }
    public class DataQueue
    {
        //À nhìn tên class là Queue thui chứ nó o có tính chất của queue đâu

        class Data
        {
            public int ThreatIndex { get; set; }
            public byte[] Info { get; set; }
            public int stats { get; set; }
            public DataType Type { get; set; }
            public byte RoomId { get; set; }
            public Data() { }
            public Data(int stast, int thread, byte[] data)
            {
                this.stats = stats;
                this.ThreatIndex = thread;
                this.Info = data;
            }
            /*
            Threadindex : Nhằm phân biệt các tác vụ khác nhau ( vai trò giống như số Port trong mạng )
       Stats: Gồm 3 trạng thái chính 

            2: bắt đầu gửi, lúc này sẽ gửi Info là dung lượng file
                lúc này 4 byte tiếp theo sẽ lưu trọng lượng vs 01 52 11 13 -> 01521113 byte ~ 1.45 mb
                kém thep type ( 1 byte ) định dạng loại file, Roomid xác dịnh phòng ( room ) và ứng dụng sẽ gửi mess
            1: đang gửi và còn nữa
            0: đợt cuối và o gửi nữa

            Tuy nhiên khi ở list lưu trữ thì stats mang ý nghĩa là vị trí tiếp theo gép mảng vào

            type : loại tin nhắn
            RoomId : xác định tin nhắn này nằm trong khung chat nào
            thứ tự là 
            | stats | threatIndex | info <- dữ liệu chính | type | RoomId |
               1byte    1byte            sizeofdata         1byte  1byte

             */
        }

        List<Data> queue = new List<Data>();

        Socket sender;
        protected int sizeofdata;// số byte dữ liệu ở mỗi lần gửi
        object Synclock = new object(); // dòng để khóa chi tiết xem ở threadpooling

        public DataQueue()
        {

        }

        public DataQueue(int size)
        {
            sizeofdata = size;
        }


        public void Extract_n_Pull(byte[] data)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            Extract_n_Pull(socket, data);
        }

        /// <summary>
        /// Nhận dữ liệu được truyền đến
        /// </summary>
        /// <param name="data">dữ liệu thô</param>
        public void Extract_n_Pull(Socket socket, byte[] data)
        {
            sender = socket;
            Data _data = new Data();
            _data.stats = data[0];
            _data.ThreatIndex = data[1];
            if (_data.stats == 2)
            {   //Chỉnh sửa cái này nếu có update tăng số lượng cilent 
                //tiến hành khởi tạo 1 vị trí trong queue 
                int _size = data[2] * 1000000 + data[3] * 10000 + data[4] * 100 + data[5] + 1;
                _data.Info = new byte[_size];
                _data.stats = 0;
                _data.Type = (DataType)data[6];
                _data.RoomId = data[7];
                queue.Add(_data);

            }
            else
            if (_data.stats == 1)
            {
                //ThreadPool.QueueUserWorkItem(pull, data);
                pull(data);
                // xem ThreadPool trong thread để biết thêm chi tiết
            }
            else
            if (_data.stats == 0)
            {
                //   ThreadPool.QueueUserWorkItem(gather, data);
                gather(data);
            }
        }

        //Hàm gom dữ liệu
        private void pull(object data)
        {

            byte[] _data = data as byte[];
            int _threadid = _data[1];
            foreach (Data dataitem in queue)// tìm từng tiến trình đang lưu
            {
                if (dataitem.ThreatIndex == _threadid)
                {
                    lock (Synclock)
                    {
                        int count = 0;
                        while (count < sizeofdata) // gắn zô thui
                        {
                            dataitem.Info[dataitem.stats] = _data[2 + count];
                            count++;
                            dataitem.stats++;
                        }
                    }

                }
            }
        }

        //Hàm gom dữ liệu lần cuối cùng do dữ liệu có thể o toàn vẹn ( có dung lượng < size )
        // Cơ bản nó thì o khác mấy so vs pull nhưng tương lai mình sẽ thêm 1 cố thông số ở cuối byte dữ liệu ( như địa chỉ ip cần gửi , loại file .. ) nên về sau sửa tiện hơn
        private void gather(object data)
        {
            byte[] _data = data as byte[];
            byte _threadid = _data[1];
            foreach (Data dataitem in queue)
            {
                if (dataitem.ThreatIndex == _threadid)
                {
                    int sizeoflastdata = dataitem.Info.Length % sizeofdata;
                    lock (Synclock)
                    {
                        int count = 0;
                        while (count < sizeoflastdata)
                        {
                            dataitem.Info[dataitem.stats] = _data[2 + count];
                            count++;
                            dataitem.stats++;
                        }
                    }

                    We_Have_Data_here(sender, dataitem.Info, _threadid, dataitem.Type, dataitem.RoomId);
                    queue.Remove(dataitem);
                    break;
                }

            }

        }

        protected virtual void We_Have_Data_here(Socket sender, byte[] data, byte index, DataType Type, byte RoomId) { }

    }

    class PhanManh
    {
        int sizeofdata;
        int numerofrow = 0;
        byte Index;
        Socket socket;
        DataType type;
        byte RoomId;

        public PhanManh(int size, byte Index, Socket socket, DataType type, byte RoomId)
        {
            sizeofdata = size;
            this.socket = socket;
            this.Index = Index;
            this.type = type;
            this.RoomId = RoomId;
        }

        /// <summary>
        /// Gửi dũ liệu thô đi
        /// </summary>
        /// <param name="data"></param>
        public void DivideAndSend(byte[] data)
        {
            int datalength = data.Length - 1;
            int remain = datalength;
            byte[] temp = new byte[8];
            temp[2] = (byte)(datalength / 1000000);
            temp[3] = (byte)((datalength - temp[2] * 1000000) / 10000);
            temp[4] = (byte)((datalength - temp[2] * 1000000 - temp[3] * 10000) / 100);
            temp[5] = (byte)(datalength - temp[2] * 1000000 - temp[3] * 10000 - temp[4] * 100);
            temp[6] = (byte)type;
            temp[7] = RoomId;
            //chia dung lượng dữ liệu ra để lưu vào mảng byte ,vd 9875 => 00 00 98 75
            send(2, Index, temp);
            while (remain > sizeofdata)
            {
                temp = new byte[sizeofdata + 2];
                for (int i = 0; i < sizeofdata; i++)
                {
                    temp[i + 2] = data[datalength - remain];
                    remain--;
                }
                send(1, Index, temp);
            }

            temp = new byte[remain + 2 + 1]; // cộng 1 vào là lúc này remain đã bị trừ khi ở đoạn gửi trên ( cắt xong là tr72 ) nên h cộng ngc lại
            int j = 0;
            while (remain >= 0)
            {
                temp[j + 2] = data[datalength - remain];
                j++;
                remain--;
            }
            send(0, Index, temp);
        } 

        private void send(byte stats, byte ThreadingId, byte[] data)
        {
            data[0] = stats;
            data[1] = ThreadingId;
            socket.Send(data);
        }
    }
}
