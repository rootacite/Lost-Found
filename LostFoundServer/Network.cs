using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LostFoundServer;

public static class NetworkExtensions
{
    /// <summary>
    /// 从客户端获取一行数据
    /// </summary>
    /// <param name="Stream"></param>
    /// <returns></returns>
    public static string ReadLine(this NetworkStream Stream)
    {
        List<byte> ret = new List<byte>();

        while(true)
        {
            int cc = Stream.ReadByte();
            if(!Stream.Socket.Connected)
                throw new Exception("DisConnected");
            if (cc == -1)
                throw new Exception("DisConnected");

            ret.Add((byte)cc);

            string sn = Encoding.UTF8.GetString(ret.ToArray());
            if(sn.EndsWith("\n", StringComparison.Ordinal))
            {
                return sn;
            }
        }
    }
}

class NetworkManager
{
    public delegate string PacketHandler(string data);
    public event PacketHandler ?Packet;
    private readonly DataStructure normalStructure = new() { Command = 0, Name = "Nike", Payload = "NoProcess" };
    public readonly TcpListener listener;

    private void ClientProcess(TcpClient cl)
    {
        var Stream = cl.GetStream();
        while (true)
        {
            try
            {
                string Data = Stream.ReadLine();       //从客户端等待一行Json数据
                string ?Reply = Packet?.Invoke(Data);  //获取处理器的返回

                if (Reply != null)                     //如果返回为非空
                {
                    //向客户端返回处理的数据
                    byte[] Dt = Encoding.UTF8.GetBytes(Reply);
                    Stream.Write(Dt);
                }
                else
                {
                    //返回默认数据
                    byte[] Dt = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(normalStructure) + "\n");
                    Stream.Write(Dt);
                }
            }
            catch (Exception)
            {
                //出现异常则放弃此客户端
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Client IP:{cl.Client.RemoteEndPoint} is Disconnected.]");
                Console.ForegroundColor = ConsoleColor.White;
                break;
            }
        }
    }

    //开始接受客户端
    public async Task Run()
    {
        listener.Start();
        while(true)
        {
            try
            {
                //接受客户端
                TcpClient client = await listener.AcceptTcpClientAsync();
                //开启一个新的线程处理这个客户端
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"New Client Connected, IP:{client.Client.RemoteEndPoint}");
                Console.ForegroundColor = ConsoleColor.White;
                _ = Task.Run(() => ClientProcess(client));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                break;
            }
        }
    }

    public NetworkManager()
    {
        listener = new TcpListener(IPAddress.Any, 34421);
    }
}
