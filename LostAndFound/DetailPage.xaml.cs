namespace LostAndFound;

public partial class DetailPage : ContentPage
{

    public static DetailPage Instance { get; private set; }
    public DetailPage()
	{
		InitializeComponent();

        //���ض���������
        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetBackButtonTitle(this, null);

        Instance = this;
	}
}