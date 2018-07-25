using FoosballXamarin.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoosballXamarin
{
	public partial class App : Application
	{
        //public static string ApiUrl = "https://foosballapi.azurewebsites.net/api/";
        public static string ServerUrl = "http://localhost:5000/";
        public static string ApiUrl = ServerUrl + "api/";

        public static INavigation Navigation { get; set; }

        public App()
		{
			InitializeComponent();

		    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTAzMjBAMzEzNjJlMzIyZTMwaHJPSFdxU01JZnY1VEsxRG1xMG1XODMxWkVtNzVsK1lBTmQzRXRRRkl4MD0=");

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
                        Title = "Account",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                    new NavigationPage(new AdministrationPage())
                    {
                        Title = "Admin",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                }
            };

		    Navigation = Current.MainPage.Navigation;
		}
	}
}
