using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cy_Connection_Sever
{

    internal struct FileInfoItem
    {
        public int index;
        public string name;
    }

    static class FileControler
    {
        public static string lastname;
        public static List<FileInfoItem> ListFile = new List<FileInfoItem>();

        private static string path = AppDomain.CurrentDomain.BaseDirectory;
        private static object lockobj = new object();

        public static void setLastFileName(string name)
        {
            lastname = name;
        }

        public static int Savefile(byte[] data)
        {
            lock (lockobj)
            {
                FileInfoItem item;
                item.index = ListFile.Count + 1;
                item.name = lastname;
                ListFile.Add(item);
                DataConverter.Deserialize_File(data, path, item.index.ToString());
                return item.index;
            }
        }

        public static byte[] loadfile(string indexstr)
        {
            //bổ sung thêm kiểm tra file có tồn tại
            string fullname = path + "\\" + indexstr;
            return DataConverter.Serialize_File(fullname);
        }

        public static string loadName(int idex)
        {
            foreach (FileInfoItem item in ListFile)
            {
                if (item.index == idex)
                {
                    return item.name;
                }
            }
            return "404 File not found";
        }
    }
}
