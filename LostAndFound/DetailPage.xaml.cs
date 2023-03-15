namespace LostAndFound;

public partial class DetailPage : ContentPage
{
	public DetailPage()
	{
		InitializeComponent();

		Loaded += (e, v) =>
		{

			NavigationPage.SetHasNavigationBar(this, false);
			NavigationPage.SetBackButtonTitle(this, null);
		};
	}
}