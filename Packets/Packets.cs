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

            int readLengthStartIndex = 0;

            for(int i = 0; i < content.Length; i++)
            {
                if (Packet.ValidateType(content[i]))
                {
                    int length = Int32.Parse(content.Substring(readLengthStartIndex, i - readLengthStartIndex));
                    Packet.DataType type = (Packet.DataType)content[i];

                    string subcontent = content.Substring(i + 1, length);
                    result.Add(new Tuple<Packet.DataType, string>(type, subcontent));

                    i = i + 1 + length;
                    readLengthStartIndex = i;
                }
            }

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
                if (!socket.Connected)
                    return;

                socket.Send(packet.GetData());
            });
        }

        public static List<Tuple<Packet.DataType, string>> Decode(int bytes, ref StringBuilder sb, ref byte[] buffer)
        {
            sb.Append(Encoding.ASCII.GetString(buffer, 0, bytes));
            string decodedString = sb.ToString();
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