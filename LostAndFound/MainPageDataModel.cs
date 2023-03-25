  
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Drawing;
using Android.OS;
using SkiaSharp;
using Android.Hardware.Lights;
using static Android.Icu.Text.ListFormatter;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.Arm;

namespace LostAndFound
{
    public partial class MainPageDataModel : ObservableObject
    {

        public ObservableCollection<ItemInfo> itemInfos { get; set; } = new ObservableCollection<ItemInfo>();

        [RelayCommand]
        async void OnClick1()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    using Stream sourceStream = await photo.OpenReadAsync();

                    byte[] Data_load = new byte[sourceStream.Length];

                    sourceStream.Read(Data_load, 0, (int)sourceStream.Length);
                   

                    await Task.Delay(500);
                    await MainPage.Instance.Navigation.PushAsync(new ManwPage(Data_load), true);
 
             
                }
            }
        }

        [RelayCommand]
        async void OnClickFinding()
        {
            await MainPage.Instance.Navigation.PushAsync(new FindingPage(), true);
        }

        [RelayCommand]
        async void OnFresh()
        {
            try
            {
                DataStructure reply = await ClientMobel.GetReply(new DataStructure()
                {
                    Command = 0,
                    Name = "Items",
                    Payload = "0"
                });

                if (reply.Command == 1)
                {
                    await MainPage.Instance.DisplayAlert("Messag", "No Data", "OK");
                    return;
                }
                else
                {
                    ObservableCollection<ItemInfo> lib_sr = JsonConvert.DeserializeObject<ObservableCollection<ItemInfo>>(reply.Payload);
                    itemInfos.Clear();
                    foreach (var i in lib_sr)
                    {
                        itemInfos.Add(i);
                    }
                }
            }
            catch(Exception ex)
            {
                await MainPage.Instance.DisplayAlert("Error", ex.Message, "Close");
            }
        }

        /// <summary>
        /// 点击列表中项目触发的事件
        /// </summary>
        /// <param name="Info"></param>
        [RelayCommand]
        async void OnClickItem(ItemInfo Info)
        {

            //加载详情页
            var DP = new DetailPage();
            var DPD = new DetailPageDataModel();

            DPD.Description = Info.Description;
            DPD.Name = Info.Name;
            DPD.Icon = Info.Icon;

            DP.BindingContext = DPD;

            //显示详情页，并等待用户将其关闭
            await MainPage.Instance.Navigation.PushAsync(DP, true);
        }

        [ObservableProperty]
        string entryText1;

        public MainPageDataModel()
        {
            Instance = this;
        }

        public static MainPageDataModel Instance { get; private set; }
    }
}
