using FoosballXamarin.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoosballXamarin
{
	public partial class App : Application
	{
        //public static string ApiUrl = "http://foosball9000api.sovs.net/api/";
        public static string ApiUrl = "http://staging-foosball9000api.sovs.net/api/";

        public static INavigation Navigation { get; set; }

        public App()
		{
			InitializeComponent();

			SetMainPage();
		}

		public static void SetMainPage()
		{
            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Browse",
                        Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                    new NavigationPage(new StartMatchPage())
                    {
                        Title = "Add Match",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                }
            };

		    Navigation = Current.MainPage.Navigation;
		}
	}
}
