
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LostFoundServer;

class Program
{
    private static readonly DataStructure normalStructure = new() { Command = 0, Name = "Nike", Payload = "Error" };

    public static string DataProcess(string s)
    {
        try
        {
            DataStructure ?ds = JsonConvert.DeserializeObject<DataStructure>(s); //将接收到的Json字符串反序列化成数据类
            if (ds == null) throw new Exception("No Available"); //如果反序列化失败则抛出异常

            switch(ds.Command)
            {
                case 0:
                    {
                        //读命令
                        if(!Data.Exists(ds.Name))
                        {
                            //如果不存在该数据，则直接返回
                            return JsonConvert.SerializeObject(new DataStructure() { Command = 0, Name = ds.Name, Payload = "No Data" }) + "\n";
                        }

                        var ret = Data.Get(ds.Name);
                        Console.WriteLine($"Gotten Item : {ds.Name}, Returned {ret}");
                        return ret + "\n";
                    }

                case 1:
                    {
                        //写命令
                        bool r = Data.Set(ds.Name, ds.Payload);
                        Data.Save(); //更新文件数据

                        if (r)
                        {
                            //更新操作
                            Console.WriteLine($"Updated Data : {ds.Name} to {ds.Payload}");
                            return JsonConvert.SerializeObject(new DataStructure() { Command = 1, Name = ds.Name, Payload = "Updated" }) + "\n";
                        }
                        else
                        {
                            //添加操作
                            Console.WriteLine($"Added New Data : {ds.Name} with {ds.Payload}");
                            return JsonConvert.SerializeObject(new DataStructure() { Command = 1, Name = ds.Name, Payload = "Added" }) + "\n";
                        }
                    }
                case 3:
                    {
                        //删除命令
                        Data.Delete(ds.Name);
                        Data.Save();
                        Console.WriteLine($"Deleted Data : {ds.Name}");
                        return JsonConvert.SerializeObject(new DataStructure() { Command = 3, Name = ds.Name, Payload = "Deleted" }) + "\n";
                    }
                default:
                    {
                        //错误的命令，抛出异常
                        throw new Exception("Undefined Command!");
                    }
            }
        }
        catch (Exception)
        {
            return JsonConvert.SerializeObject(normalStructure) + "\n";
        }
    }

    private static DataManager Data = new DataManager();
    private static NetworkManager Network = new NetworkManager();
    public static void Main(string[] argv)
    {
        Data.Load();
        Data.Print();

        Network.Packet += DataProcess;
        _ = Network.Run();

        Console.CancelKeyPress += (e, v) =>
        {
            Network.listener.Stop();
        };
        while (true)
        {
            string? mc = Console.ReadLine();

            if(mc != null)
            {
                if (mc == "q")
                {
                    Network.listener.Stop();
                }

                if(mc == "p") 
                {
                    Data.Print();
                }
            }
        }
    }
}