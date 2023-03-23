namespace LostAndFound;

public partial class DetailPage : ContentPage
{

    public static DetailPage Instance { get; private set; }
    public DetailPage()
	{
		InitializeComponent();

        //Òþ²Ø¶¥²¿µ¼º½À¸
        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetBackButtonTitle(this, null);

        Instance = this;
	}
}