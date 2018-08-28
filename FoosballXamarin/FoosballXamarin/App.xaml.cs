using Xamarin.Forms.Xaml;
using FoosballXamarin.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoosballXamarin
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTAzMjBAMzEzNjJlMzIyZTMwaHJPSFdxU01JZnY1VEsxRG1xMG1XODMxWkVtNzVsK1lBTmQzRXRRRkl4MD0=");
            if(!Preferences.ContainsKey("ApiUrlSettings"))
            {
                MainPage = new NavigationPage(new UrlLandingPage());
            }
            else
            {
                MainPage = new MainPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
