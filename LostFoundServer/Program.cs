
using System;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LostFoundServer;

class Program
{
    private static DataManager Manager = new DataManager();
    public static void Main(string[] argv)
    {
        Manager.Load();

        Manager.Print();
    }
}