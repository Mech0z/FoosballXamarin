using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FoosballXamarin.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoosballXamarin
{
    public partial class App : Application
    {
        public static string ServerUrl = "http://betafoosballapi.azurewebsites.net/";
        //public static string ServerUrl = "http://localhost:5000/";
        public static string ApiUrl = ServerUrl + "api/";

        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTAzMjBAMzEzNjJlMzIyZTMwaHJPSFdxU01JZnY1VEsxRG1xMG1XODMxWkVtNzVsK1lBTmQzRXRRRkl4MD0=");

            MainPage = new MainPage();
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
