
using Microsoft.Maui.Controls.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LostAndFound
{
    public partial class MainPageDataModel : ObservableObject
    {
        public List<ItemInfo> itemInfos { get; set; } = new List<ItemInfo>();

        [RelayCommand]
        async void OnClick1()
        {
           await MainPage.Instance.Navigation.PushAsync(new DetailPage());
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
