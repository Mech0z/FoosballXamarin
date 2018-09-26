using System;
using FoosballXamarin.ViewModels;
using Xamarin.Forms.Xaml;
using FoosballXamarin.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoosballXamarin
{
    public partial class App
    {
        private const string Apiurlsettings = "ApiUrlSettings";
        public HubConnection HubConnection { get; set; }

        public App()
        {
            InitializeComponent();
            
            //Preferences.Clear();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTAzMjBAMzEzNjJlMzIyZTMwaHJPSFdxU01JZnY1VEsxRG1xMG1XODMxWkVtNzVsK1lBTmQzRXRRRkl4MD0=");
            if(!Preferences.ContainsKey(Apiurlsettings))
            {
                MainPage = new NavigationPage(new UrlLandingPage());
            }
            else
            {
                var api = Preferences.Get(Apiurlsettings, "");
                MigrateUrl(api);
                
                MainPage = new MainPage();
            }

            SetupSignalRHubs();

            MessagingCenter.Subscribe<UrlLandingViewModel>(this, "ApiPingSuccess", (sender) => SetupSignalRHubs());
        }

        private void MigrateUrl(string currentSetting)
        {
            currentSetting = Trim(currentSetting, "http://");
            currentSetting = Trim(currentSetting, "https://");
            currentSetting = Trim(currentSetting, "/api/");

            Preferences.Set(Apiurlsettings, currentSetting);
        }

        private string Trim(string stringToTrim, string trimText)
        {
            return stringToTrim.Contains(trimText)
                ? stringToTrim.Remove(stringToTrim.IndexOf(trimText, StringComparison.Ordinal), trimText.Length)
                : stringToTrim;
        }
        
        private void SetupSignalRHubs()
        {
            if (Preferences.ContainsKey(Apiurlsettings))
            {
                var url = Preferences.Get(Apiurlsettings, "");

                HubConnection = new HubConnectionBuilder()
                    .WithUrl("https://" + url + "/matchAddedHub")
                    .Build();

                HubConnection?.On<string, string>("matchAdded",
                    (user, message) => { MessagingCenter.Send(this, "SignalR-MatchAdded"); });
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
