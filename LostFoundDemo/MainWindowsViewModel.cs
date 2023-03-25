using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Newtonsoft.Json;

namespace LostFoundDemo
{
    struct DataStructure
    {
        public int Command;
        //可以是 0，1，2，3，4 0:读取 1:写入 2:表示Payload是一个Base64编码的文件，需要被保存 3:删除名称为Name的数据 4:删除文件名为Name的文件
        public string Name;
        //数据的名称
        public string Payload;
        //负载数据
    }


    public partial class MainWindowsViewModel : ObservableObject
    {
        public MainWindowsViewModel()
        {
            ConnectEnable = true;
            Command = "";
            Name = "";
            Payload = "";
        }

        [ObservableProperty]
        string command;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string payload;


        [ObservableProperty]
        bool connectEnable;

        DataStructure DataStructure
        {
            get
            {
                return new DataStructure() 
                { 
                    Name = Name,
                    Payload = Payload,
                    Command = Convert.ToInt32(Command)
                };
            }
        }



        [RelayCommand]
        async void OnClickSend()
        {

            NetworkClient _networkClient = new NetworkClient();
            await _networkClient.Connect();

            string Data = JsonConvert.SerializeObject(DataStructure);

            string Reply = await _networkClient.GetReply(Data);

            _networkClient.DisConnect();
            Payload = Reply;
        }
    }
}
