
using System;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LostFoundServer;

class Program
{
    private static DataManager Data = new DataManager();
    private static NetworkManager Network = new NetworkManager();
    public static void Main(string[] argv)
    {
        Data.Load();

        Data.Print();

        Network.Packet += (s) =>
        {
            return $"{s} Received\n";
        };
        _ = Network.Run();

        while (true)
        {
            string? mc = Console.ReadLine();

            if(mc != null)
            {
                if (mc == "q")
                {
                    Network.listener.Stop();
                }
            }
        }
    }
}