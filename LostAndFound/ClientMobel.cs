using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound
{
    public struct DataStructure
    {
        public int Command;
        //可以是 0，1，2，3，4 0:读取 1:写入 2:表示Payload是一个Base64编码的文件，需要被保存 3:删除名称为Name的数据 4:删除文件名为Name的文件
        public string Name;
        //数据的名称
        public string Payload;
        //负载数据
    }


    public static class NetworkExtensions
    {
        /// <summary>
        /// 从客户端获取一行数据
        /// </summary>
        /// <param name="Stream"></param>
        /// <returns></returns>
        public static string ReadLine(this NetworkStream Stream)
        {
            List<byte> ret = new();

            while (true)
            {
                int cc = Stream.ReadByte();
                if (!Stream.Socket.Connected)
                    throw new Exception("DisConnected");
                if (cc == -1)
                    throw new Exception("DisConnected");

                ret.Add((byte)cc);

                string sn = Encoding.UTF8.GetString(ret.ToArray());
                if (sn.EndsWith("\n", StringComparison.Ordinal))
                {
                    return sn;
                }
            }
        }
    }

    public class ClientMobel
    {
        public ClientMobel()
        {
            
        }

        public async static Task<DataStructure> GetReply(DataStructure Data)
        {
            return JsonConvert.DeserializeObject<DataStructure>
                (
                    await Quick(JsonConvert.SerializeObject(Data))
                );
        }

        public async static Task<string> Quick(string Data)
        {
            return await Task.Run(() =>
            {

                using TcpClient tcpClient = new();
                tcpClient.Connect("59.110.225.239", 34421);
                using NetworkStream stream = tcpClient.GetStream();

                byte[] Buffer = Encoding.UTF8.GetBytes(Data + "\n");
                stream.Write(Buffer, 0, Buffer.Length);

                string ret = stream.ReadLine();

                stream.Close();
                tcpClient.Close();

                return ret;
            });
        }
    }
}
