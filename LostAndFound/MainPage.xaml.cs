
using System.ComponentModel;

namespace LostAndFound;

public class ItemInfo
{
    public string Name { get; set; }
    public string Description { get; set; }

    public string Icon { get; set; }

}

public partial class MainPage : ContentPage
{
	static public MainPage Instance { get; private set; }


	public MainPage()
	{
		InitializeComponent();
		Instance = this;
        BindingContext = new MainPageDataModel();

        var itemInfos = (BindingContext as MainPageDataModel).itemInfos;

        Loaded += (e, v) =>
		{

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, null);

            itemInfos.Add(new ItemInfo() { Name = "书包", Description = "一个红色的书包，2022.1.12于南食堂二楼捡到", Icon = "https://img2.baidu.com/it/u=4015522598,3286453700&fm=253&fmt=auto&app=138&f=JPEG?w=500&h=521" });
            itemInfos.Add(new ItemInfo() { Name = "飞机", Description = "一架波音747飞机，2022.3.35坠毁于太平洋", Icon = "https://img1.baidu.com/it/u=1280330502,2210838467&fm=253&fmt=auto&app=120&f=JPEG?w=644&h=500" });
            itemInfos.Add(new ItemInfo() { Name = "小行星", Description = "一个直径大致5km的小行星，2934.1.23被不明星系遗弃在M78星云", Icon = "https://n.sinaimg.cn/sinacn/w1419h1000/20180127/46b0-fyqzcxh3598060.jpg" });

            itemInfos.Add(new ItemInfo() { Name = "书包", Description = "一个红色的书包，2022.1.12于南食堂二楼捡到", Icon = "https://img2.baidu.com/it/u=4015522598,3286453700&fm=253&fmt=auto&app=138&f=JPEG?w=500&h=521" });
            itemInfos.Add(new ItemInfo() { Name = "飞机", Description = "一架波音747飞机，2022.3.35坠毁于太平洋", Icon = "https://img1.baidu.com/it/u=1280330502,2210838467&fm=253&fmt=auto&app=120&f=JPEG?w=644&h=500" });
            itemInfos.Add(new ItemInfo() { Name = "小行星", Description = "一个直径大致5km的小行星，2934.1.23被不明星系遗弃在M78星云", Icon = "https://n.sinaimg.cn/sinacn/w1419h1000/20180127/46b0-fyqzcxh3598060.jpg" });

            itemInfos.Add(new ItemInfo() { Name = "书包", Description = "一个红色的书包，2022.1.12于南食堂二楼捡到", Icon = "https://img2.baidu.com/it/u=4015522598,3286453700&fm=253&fmt=auto&app=138&f=JPEG?w=500&h=521" });
            itemInfos.Add(new ItemInfo() { Name = "飞机", Description = "一架波音747飞机，2022.3.35坠毁于太平洋", Icon = "https://img1.baidu.com/it/u=1280330502,2210838467&fm=253&fmt=auto&app=120&f=JPEG?w=644&h=500" });
            itemInfos.Add(new ItemInfo() { Name = "小行星", Description = "一个直径大致5km的小行星，2934.1.23被不明星系遗弃在M78星云", Icon = "https://n.sinaimg.cn/sinacn/w1419h1000/20180127/46b0-fyqzcxh3598060.jpg" });

            CollList.ItemsSource = itemInfos;
        };
	}
}

