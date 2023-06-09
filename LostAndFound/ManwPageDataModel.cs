﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;


namespace LostAndFound
{
    public partial class ManwPageDataModel : ObservableObject
    {
        private readonly MemoryStream _stream;
        private readonly byte[] _data;


        [ObservableProperty]
        ImageSource tcSource;

        [ObservableProperty]
        string titleModel;

        [ObservableProperty]
        string desModel;

        [RelayCommand]
        async void OnTransmit()
        {
            await Process(_data);
            ManwPage.Instance.Close();
        }

        public ManwPageDataModel(byte[] data)
        {
            _data = (byte[])data.Clone();
            _stream = new MemoryStream(_data);
            TcSource = ImageSource.FromStream(() => _stream);

            ManwPage.Instance.Unloaded += (e, v) =>
            {
                _stream?.Dispose();
            };
        }

        ~ManwPageDataModel() 
        {
            
        }

        async Task Process(byte[] source)
        {

            using SKBitmap bitmap = SKBitmap.Decode(source);

            using SKBitmap scaledBitmap = bitmap.Resize(new SKImageInfo(600, 600), SKFilterQuality.Medium);
            using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);


            System.Diagnostics.Debug.WriteLine($"Rgion Pic Width : {scaledImage.Width} Height : {scaledImage.Height}");

            using (SKData dt = scaledImage.Encode(SKEncodedImageFormat.Jpeg, 50))
            {
                var b_dt = dt.ToArray();
                System.Diagnostics.Debug.WriteLine($"Data Length : {b_dt.Length}");

                /////////////////////////////////////////////////////

                var inj_scan = new QRCodeScanner();
                await ManwPage.Instance.Navigation.PushAsync(inj_scan, false);
                string res = await inj_scan.WaitResult();
                await ManwPage.Instance.Navigation.PopAsync();
                await Task.Delay(500);
                

                MainPageDataModel.Instance.itemInfos.Add(new ItemInfo()
                {
                    Icon = "http://59.110.225.239/" + TitleModel + ".jpg",
                    Name = TitleModel,
                    Description = DesModel,
                    Tag = res
                });


                var reply = await ClientMobel.GetReply(new DataStructure()
                {
                    Command = 2,
                    Name = TitleModel + ".jpg",
                    Payload = Convert.ToBase64String(b_dt)
                });

                await ClientMobel.GetReply(new DataStructure()
                {
                    Command = 1,
                    Name = "Items",
                    Payload = JsonConvert.SerializeObject(MainPageDataModel.Instance.itemInfos)
                });



            }
        }
    }
}
