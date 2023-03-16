namespace LostAndFound;

public partial class DetailPage : ContentPage
{
	public DetailPage()
	{
		InitializeComponent();

		Loaded += (e, v) =>
		{
			//Òþ²Ø¶¥²¿µ¼º½À¸
			NavigationPage.SetHasNavigationBar(this, false);
			NavigationPage.SetBackButtonTitle(this, null);
		};
	}
}