namespace LostAndFound;

public partial class DetailPage : ContentPage
{
	public DetailPage()
	{
		InitializeComponent();

        //���ض���������
        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetBackButtonTitle(this, null);
        
	}
}