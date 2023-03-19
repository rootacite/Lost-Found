
using System.ComponentModel;
using System.Net;

namespace LostAndFound;

public class ItemInfo
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ImageSource Icon { get; set; }

}

public partial class MainPage : ContentPage
{
	static public MainPage Instance { get; private set; }


    public MainPage()
    {
        InitializeComponent();
        Instance = this;
        BindingContext = new MainPageDataModel();

        var itemInfos = (BindingContext as MainPageDataModel).itemInfos;

        Loaded += (e, v) =>
        {

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, null);


            itemInfos.Add(new ItemInfo() { Name = "TestDemo", Icon = "http://img2.baidu.com/it/u=4015522598,3286453700&fm=253&fmt=auto&app=138&f=JPEG?w=500&h=521", Description = "None" });
            itemInfos.Add(new ItemInfo() { Name = "TestDemo2", Icon = "http://59.110.225.239/496b4983caa043d3bcecc285b437d424.jpg", Description = "None" });

        };
    }
}

