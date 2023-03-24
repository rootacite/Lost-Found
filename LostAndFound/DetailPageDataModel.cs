using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound
{
    public partial class DetailPageDataModel : ObservableObject
    {
        //分别对应详情页中的名称，描述，图标

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string description;

        [ObservableProperty]
        public ImageSource icon;

        [RelayCommand]
        async void OnClaim()
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                //name
                for (int i = 0; i < MainPageDataModel.Instance.itemInfos.Count; i++)
                {
                    if (MainPageDataModel.Instance.itemInfos[i].Name == this.Name)
                    {
                        MainPageDataModel.Instance.itemInfos.RemoveAt(i);
                        break;
                    }
                }

                await ClientMobel.GetReply(new DataStructure()
                {
                    Command = 1,
                    Name = "Items",
                    Payload = JsonConvert.SerializeObject(MainPageDataModel.Instance.itemInfos)
                });
                await ClientMobel.GetReply(new DataStructure()
                {
                    Command = 5,
                    Name = "13019244532",
                    Payload = "李四 230409200405062314"
                });

                _ = Task.Run(() =>
                {
                    try
                    {
                        TcpClient tcLock = new();
                        tcLock.Connect("59.110.225.239", 34420);

                        var tcStream = tcLock.GetStream();
                        tcStream.Write(Encoding.UTF8.GetBytes("s0"));

                        byte[] Buffer = new byte[8];

                        tcStream.Read(Buffer, 0, 8);

                        tcStream.Close();
                        tcLock.Close();
                    }
                    catch (Exception)
                    {

                    }
                });

                await DetailPage.Instance.Navigation.PopAsync();
            }

            await Task.Delay(500);
           
        }
    }
}
