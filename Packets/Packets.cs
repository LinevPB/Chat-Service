using System.Text;
using System.Net.Sockets;

namespace App.Packets
{
    public partial class Packet
    {
        private const int BYTE_SIZE = 2048;
        public string strData;
        private byte[] data;

        public Packet()
        {
            strData = String.Empty;
            data = new byte[BYTE_SIZE];
        }

        public byte[] Parse()
        {
            data = Encoding.ASCII.GetBytes(strData);
            return data;
        }

        public byte[] GetData()
        {
            return data;
        }


        private string ConstructArg(DataType type, string value)
        {
            // eg. 4d2137
            return String.Format("{0}{1}{2}", value.Length, (char)type, value);
        }

        private static List<Tuple<Packet.DataType, string>> Deconstruct(string content)
        {
            List<Tuple<Packet.DataType, string>> result = new List<Tuple<Packet.DataType,string>>();

            bool typeKnown = false;
            bool lengthKnown = false;
            bool contentKnown = false;

            char ctype = ' ';
            string length = String.Empty;
            int i32length = 0;
            int contentStartIndex = 0;
            string subContent = String.Empty;

            //tbd

            return result;
        }

        public void WriteInt(int value)
        {
            strData += ConstructArg(DataType.Int, value.ToString());
        }

        public void WriteFloat(float value)
        {
            strData += ConstructArg(DataType.Float, value.ToString());
        }

        public void WriteString(string value)
        {
            strData += ConstructArg((DataType)DataType.String, value);
        }

        public static async void SendData(Socket socket, Packet packet)
        {
            await Task.Run(() =>
            {
                socket.Send(packet.GetData());
            });
        }

        public static List<Tuple<Packet.DataType, string>> Decode(int bytes, ref StringBuilder sb, ref byte[] buffer)
        {
            string decodedString;

            sb.Append(Encoding.ASCII.GetString(buffer, 0, bytes));
            decodedString = sb.ToString();
            sb.Clear();

            return Deconstruct(decodedString);
        }

        public static int ParseInt(string v)
        {
            return int.Parse(v);
        }

        public static float ParseFloat(string v)
        {
            return float.Parse(v);
        }

        public static string ParseString(string v)
        {
            return v;
        }

        public static Packet.DataType ParseType(char v)
        {
            return (Packet.DataType)v;
        }

        public static Packet.DataType ParseType(string v)
        {
            return (Packet.DataType)v[0];
        }

        public static int GetSize()
        {
            return BYTE_SIZE;
        }
    }
}