
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;



namespace LostFoundServer;

// Main Service : 34421
// Mqtt Service : 34420

class Program
{
    private static readonly DataStructure normalStructure = new() { Command = -1, Name = "Exception", Payload = "Error" };

    public static string DataProcess(string s)
    {
        try
        {
            DataStructure ?nbds = JsonConvert.DeserializeObject<DataStructure>(s); //将接收到的Json字符串反序列化成数据类
            if (nbds == null) throw new Exception("No Available"); //如果反序列化失败则抛出异常

            DataStructure ds = nbds.Value;
            switch (ds.Command)
            {
                case 0:
                    {
                        //读命令

                        if(!Data.Exists(ds.Name))
                        {
                            //如果不存在该数据，则直接返回
                            return JsonConvert.SerializeObject(new DataStructure() { Command = 1, Name = ds.Name, Payload = "No Data" }) + "\n";
                        }

                        var ret = Data.Get(ds.Name);
                        //Console.WriteLine($"[Gotten Item : {ds.Name}, Returned {ret}]");
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
                            Console.WriteLine($"[Updated Data : {ds.Name} to {ds.Payload}]");
                            return JsonConvert.SerializeObject(new DataStructure() { Command = 1, Name = ds.Name, Payload = "Updated" }) + "\n";
                        }
                        else
                        {
                            //添加操作
                            Console.WriteLine($"[Added New Data : {ds.Name} with {ds.Payload}]");
                            return JsonConvert.SerializeObject(new DataStructure() { Command = 0, Name = ds.Name, Payload = "Added" }) + "\n";
                        }
                    }
                case 2:
                    {
                        //保存文件命令
                        //Locate : ~/host/webapps/ROOT
                        byte[] OriginData = Convert.FromBase64String(ds.Payload);
                        File.WriteAllBytes(ds.Name, OriginData);

                        Console.WriteLine($"[Save {ds.Name} to /home/shiyuanli/host/webapps/ROOT/.]");
                        return JsonConvert.SerializeObject(new DataStructure() { Command = 0, Name = ds.Name, Payload = "Saved" }) + "\n";
                    }
                case 3:
                    {
                        //删除命令
                        Data.Delete(ds.Name);
                        Data.Save();
                        Console.WriteLine($"[Deleted Data : {ds.Name}]");
                        return JsonConvert.SerializeObject(new DataStructure() { Command = 0, Name = ds.Name, Payload = "Deleted" }) + "\n";
                    }
                case 4:
                    {
                        File.Delete(ds.Name);

                        Console.WriteLine($"[Deleted {ds.Name} from /home/shiyuanli/host/webapps/ROOT/.]");
                        return JsonConvert.SerializeObject(new DataStructure() { Command = 0, Name = ds.Name, Payload = "Deleted" }) + "\n";
                    }
                case 5:
                    {
                        Console.WriteLine($"[Gotten {ds.Payload} from {ds.Name}]");
                        return JsonConvert.SerializeObject(new DataStructure() { Command = 0, Name = ds.Name, Payload = "Echoed" }) + "\n";
                    }
                default:
                    {
                        //错误的命令，抛出异常
                        throw new Exception("Undefined Command!");
                    }
            }
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(normalStructure with {Payload =  ex.Message}) + "\n";
        }
    }

    private static DataManager Data = new DataManager();
    private static NetworkManager Network = new NetworkManager();

    public static void Main(string[] argv)
    {
        Data.Load();
        Data.Print();

        ////////////////
        ///Setup Mqtt Server
        ///////////////
        var options = new MqttServerOptionsBuilder()
          
        //Set endpoint to localhost
        .WithDefaultEndpoint()
        .WithDefaultEndpointPort(34420);
        
        var server = new MqttFactory().CreateMqttServer(options.Build());
        
        server.InterceptingPublishAsync += Server_InterceptingPublishAsync;
        server.ValidatingConnectionAsync += Server_ValidatingConnectionAsync;
        server.ClientDisconnectedAsync += Server_ClientDisconnectedAsync;

        server.StartAsync();


        ////////////////
        ///Setup Mqtt Local Client
        ///////////////

        var mqttClient = new MqttFactory().CreateMqttClient();

        var client_options = new MqttClientOptionsBuilder()
         .WithClientId("LocalHost")
         .WithTcpServer("127.0.0.1", 34420) // Port is optional
         .WithCredentials("root", "07211145141919")
        .Build();

        mqttClient.ConnectAsync(client_options, CancellationToken.None);

        mqttClient.ConnectedAsync += async (e) =>
        {
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("Sys/ServiceMsg").Build());
        };

        mqttClient.ApplicationMessageReceivedAsync += (e) =>
        {

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[ Message from Client ： {e.ClientId} on Topic : {e.ApplicationMessage.Topic}]");
            Console.WriteLine($"[ \"{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}\" ]");
            Console.ForegroundColor = ConsoleColor.White;
                   
            return Task.CompletedTask;
        };

        mqttClient.DisconnectedAsync += async (e) =>
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                await mqttClient.ConnectAsync(client_options, CancellationToken.None);
            }
            catch
            {

            }
        };


        Network.Packet += DataProcess;
        _ = Network.Run();

        Console.CancelKeyPress += (e, v) =>
        {
            Network.listener.Stop();
            server.StopAsync();
            mqttClient.Dispose();
        };

        while (true)
        {
            Thread.Sleep(1000);
            if(!server.IsStarted)
            {
                server.StartAsync();
            }
        }
    }
    private static Task Server_ClientDisconnectedAsync(ClientDisconnectedEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[ Client Id : {e.ClientId} Has Disconnected from Server ]");
        Console.ForegroundColor = ConsoleColor.White;

        return Task.CompletedTask;
    }


    private static Task Server_ValidatingConnectionAsync(ValidatingConnectionEventArgs e)
    {
        if (e.Password != "07211145141919" || e.UserName != "root")
        {
            e.ReasonCode = MqttConnectReasonCode.NotAuthorized;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[ A Connection from Client : {e.ClientId} Has been Refused Because of Invalid Authentication Information ]");
            Console.WriteLine($"[ IP Address : {e.Endpoint} ]");
            Console.ForegroundColor = ConsoleColor.White;

            return Task.FromException(new Exception("User not authorized"));
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[ New Client : {e.ClientId} Has Connected Successfully ]");
        Console.WriteLine($"[ IP Address : {e.Endpoint} ]");
        Console.ForegroundColor = ConsoleColor.White;

        return Task.CompletedTask;
    }

    
    private static Task Server_InterceptingPublishAsync(InterceptingPublishEventArgs arg)
    {
        // Convert Payload to string
        var payload = arg.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(arg.ApplicationMessage?.Payload);

        Console.WriteLine($"[ Gotten Topic : {arg.ApplicationMessage?.Topic} with {payload} ]");

        return Task.CompletedTask;
    }
}