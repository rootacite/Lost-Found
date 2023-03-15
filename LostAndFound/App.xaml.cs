namespace LostAndFound;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		var nn = new NavigationPage(new MainPage());

        MainPage = nn;
	}
}
