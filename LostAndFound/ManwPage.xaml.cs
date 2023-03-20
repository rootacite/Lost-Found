using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace LostAndFound;

public partial class ManwPage : ContentPage
{

    public async void Close()
    {
        await Navigation.PopAsync();
    }

    public static ManwPage Instance { get; private set; }
    public ManwPage(byte[] Tc)
	{
		InitializeComponent();
        Instance = this;
        BindingContext = new ManwPageDataModel(Tc);


        //Òþ²Ø¶¥²¿µ¼º½À¸
        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetBackButtonTitle(this, null);

        
    }
}