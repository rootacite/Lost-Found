
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;

namespace LostAndFound;

public class ItemInfo
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ImageSource Icon { get; set; }

    public string Tag { get; set; }
}

public partial class MainPage : ContentPage
{
	static public MainPage Instance { get; private set; }

    public async Task Fresh()
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
                mainPageDataModel.itemInfos.Clear();
                foreach (var i in lib_sr)
                {
                    mainPageDataModel.itemInfos.Add(i);
                }
            }
        }
        catch (Exception ex)
        {
            await MainPage.Instance.DisplayAlert("Error", ex.Message, "Close");
        }
    }

    private readonly MainPageDataModel mainPageDataModel;
    public MainPage()
    {
        InitializeComponent();
        Instance = this;
        BindingContext = new MainPageDataModel();
        mainPageDataModel = BindingContext as MainPageDataModel;

        var itemInfos = mainPageDataModel.itemInfos;
        


        Loaded += async (e, v) =>
        {

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, null);

            await Fresh();
        };
    }
}

