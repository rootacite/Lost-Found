namespace LostAndFound;

public partial class DetailPage : ContentPage
{
	public DetailPage()
	{
		InitializeComponent();

		Loaded += (e, v) =>
		{
			//���ض���������
			NavigationPage.SetHasNavigationBar(this, false);
			NavigationPage.SetBackButtonTitle(this, null);
		};
	}
}