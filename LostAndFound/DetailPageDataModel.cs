

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

                string filename = "";
                for (int i = 0; i < MainPageDataModel.Instance.itemInfos.Count; i++)
                {
                    if (MainPageDataModel.Instance.itemInfos[i].Name == this.Name)
                    {
                        string[] fs_uri = (MainPageDataModel.Instance.itemInfos[i].Icon as UriImageSource).Uri.ToString().Split('/');
                        filename = fs_uri[fs_uri.Length - 1];
                        MainPageDataModel.Instance.itemInfos[i].Tag = "已领取";
                        break;
                    }
                }

                //await ClientMobel.GetReply(new DataStructure()
                //{
                //    Command = 4,
                //    Name = filename,
                //    Payload = "Delete"
                //});

                await ClientMobel.GetReply(new DataStructure()
                {
                    Command = 1,
                    Name = "Items",
                    Payload = JsonConvert.SerializeObject(MainPageDataModel.Instance.itemInfos)
                });
              
                await ClientMobel.GetReply(new DataStructure()
                {
                    Command = 1,
                    Name = "Lock_1",
                    Payload = "1"
                });

                await DetailPage.Instance.Navigation.PopAsync();
            }

            await Task.Delay(500);
           
        }
    }
}
