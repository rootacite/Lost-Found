  
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

namespace LostAndFound
{
    public partial class MainPageDataModel : ObservableObject
    {
        public async Task TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    System.Diagnostics.Debug.WriteLine(sourceStream.Length.ToString());

                    using SKBitmap bitmap = SKBitmap.Decode(sourceStream);

                    using SKBitmap scaledBitmap = bitmap.Resize(new SKImageInfo(600, 600), SKFilterQuality.Medium);
                    using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);


                    System.Diagnostics.Debug.WriteLine($"Rgion Pic Width : {scaledImage.Width} Height : {scaledImage.Height}");

                    using (SKData dt= scaledImage.Encode(SKEncodedImageFormat.Jpeg, 50))
                    {
                        var b_dt = dt.ToArray();
                        System.Diagnostics.Debug.WriteLine($"Data Length : {b_dt.Length}");

                        var reply = await ClientMobel.GetReply(new DataStructure()
                        {
                            Command = 2,
                            Name = photo.FileName,
                            Payload = Convert.ToBase64String(b_dt)
                        });

                        itemInfos.Add(new ItemInfo()
                        {
                            Icon = "http://59.110.225.239/" + photo.FileName,
                            Name = photo.FileName,
                            Description = "None"
                        });

                        await ClientMobel.GetReply(new DataStructure()
                        {
                            Command = 1,
                            Name = "Items",
                            Payload = JsonConvert.SerializeObject(itemInfos)
                        });
                    }
                }
            }
        }

        public ObservableCollection<ItemInfo> itemInfos { get; set; } = new ObservableCollection<ItemInfo>();

        [RelayCommand]
        async void OnClick1()
        {
            await TakePhoto();
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
            await MainPage.Instance.Navigation.PushAsync(DP);
        }

        [ObservableProperty]
        string entryText1;

        public MainPageDataModel()
        {

        }
    }
}
