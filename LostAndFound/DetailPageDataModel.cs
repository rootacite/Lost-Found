﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

            await DetailPage.Instance.Navigation.PopAsync();
        }
    }
}
