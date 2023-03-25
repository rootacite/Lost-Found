using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace LostFoundDemo
{
    public static class NetworkExtensions
    {
        /// <summary>
        /// 从客户端获取一行数据
        /// </summary>
        /// <param name="Stream"></param>
        /// <returns></returns>
        public async static Task<string> ReadLine(this NetworkStream Stream)
        {
            return await Task.Run(() =>
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
            });
        }
    }
    class NetworkClient
    {
        private readonly TcpClient tcpClient;
        private NetworkStream stream;
        public NetworkClient()
        {
            tcpClient = new TcpClient();
        }

        async public Task Connect()
        {
            await tcpClient.ConnectAsync("59.110.225.239", 34421);
            stream = tcpClient.GetStream();
        }

        public void DisConnect()
        {
            stream.Close();
            tcpClient.Close();
            tcpClient.Dispose();
        }

        async public Task<string> GetReply(string Data)
        {
            byte[] Buffer = Encoding.UTF8.GetBytes(Data + "\n");
            await stream.WriteAsync(Buffer);
            return await stream.ReadLine();
        }
    }
}
