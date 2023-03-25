using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound;

public partial class FindingPageDataModel : ObservableObject
{
    public ObservableCollection<ItemInfo> itemInfos { get; set; } = new ObservableCollection<ItemInfo>();


}

