using FoosballXamarin.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoosballXamarin
{
	public partial class App : Application
	{
        public static string ApiUrl = "http://foosballapi.azurewebsites.net/api/";

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
                        Title = "Leaderboard",
                        Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
                    },
                    new NavigationPage(new LastMatchesPage())
                    {
                        Title = "Last games",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                    new NavigationPage(new StartMatchPage())
                    {
                        Title = "Add Match",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                    new NavigationPage(new LoginPage())
                    {
                        Title = "Login",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                }
            };

		    Navigation = Current.MainPage.Navigation;
		}
	}
}
