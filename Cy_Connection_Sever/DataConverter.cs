using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Drawing.Imaging;
 
namespace Cy_Connection_Sever
{
   public  static class DataConverter
    {
        public static byte[] Serialize_Text(object obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static object Deserialize_Text(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(ms);
        }

        public static byte[] Serialize_Image(object obj)
        {
            Image image = (Image)obj;

            if (image == null)
                throw new ArgumentNullException("Image Serialize Argument Null Exceoption");

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }

        }


        public static object DeSerialize_Image(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                if (data == null)
                    throw new ArgumentNullException("Image Deserialize Argument Null Exceoption");
                return Image.FromStream(stream);
            }
        }


        public static byte[] Serialize_File(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void Deserialize_File(byte[] data, string path, string filename)
        {
            filename = path + "\\" + filename;
            using (BinaryWriter binaryWrite = new BinaryWriter(File.OpenWrite(filename)))
            {
                binaryWrite.Write(data);
                binaryWrite.Flush();
                binaryWrite.Close();
            }
        }
    }

}
