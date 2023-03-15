
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

        [RelayCommand]
        async void OnClickItem(ItemInfo Info)
        {
            await MainPage.Instance.DisplayAlert(Info.Name, Info.Description, "OK");
        }

        [ObservableProperty]
        string entryText1;

        public MainPageDataModel()
        {

        }
    }
}
