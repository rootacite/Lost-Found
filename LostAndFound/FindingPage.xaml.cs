namespace LostAndFound;

public partial class FindingPage : ContentPage
{

    private readonly FindingPageDataModel findingPageDataModel;

    public FindingPage()
	{
		InitializeComponent();


        //隐藏顶部导航栏
        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetBackButtonTitle(this, null);

        BindingContext = new FindingPageDataModel();
        findingPageDataModel = BindingContext as FindingPageDataModel;

        findingPageDataModel.itemInfos.Add(new ItemInfo()
        {
            Name = "鼠标",
            Description = "我丢失了一个鼠标",
            Tag = "创新创业学院",
            Icon = "https://th.bing.com/th/id/R.1739c1b728307e9389f0b8ecb513a5a2?rik=0dElwmU%2fiwnunQ&riu=http%3a%2f%2fres-sh.clpcdn.com%2fpmspic%2fItemPicture%2f20005%2f20055%2f20357%2f48781%2fOriginal%2f48781_5680828.jpg&ehk=N%2f78VzolVXxVC%2fwYcCbk7e5iUzHaOb%2bMo1nCNuKq7FQ%3d&risl=&pid=ImgRaw&r=0"
        }
        );

        findingPageDataModel.itemInfos.Add(new ItemInfo()
        {
            Name = "电脑",
            Description = "我在教学楼A座丢失了一个笔记本电脑，归还必有重谢",
            Tag = "教学楼",
            Icon = "https://th.bing.com/th/id/R.cd462352477103ec7b6223f0014d8b7f?rik=w9cN3%2fpOGa657A&pid=ImgRaw&r=0"
        }
        );

        findingPageDataModel.itemInfos.Add(new ItemInfo()
        {
            Name = "水杯",
            Description = "我在图书馆丢失了一个柱状的水杯",
            Tag = "图书馆",
            Icon = "https://th.bing.com/th/id/R.c2a035c7d23d7c512206f38703e450b5?rik=hFug36EwS5tKYA&pid=ImgRaw&r=0"
        }
        );
    }
}