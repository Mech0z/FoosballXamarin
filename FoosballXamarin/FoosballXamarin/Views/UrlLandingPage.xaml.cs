using System;
using System.Threading.Tasks;
using FoosballXamarin.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UrlLandingPage
    {
        private readonly UrlLandingViewModel _viewModel;

        public UrlLandingPage()
        {
            InitializeComponent();

            _viewModel = new UrlLandingViewModel();

            MessagingCenter.Subscribe<UrlLandingViewModel>(this, "ApiPingSuccess",
                sender => { ((App)Application.Current).MainPage = new MainPage(); });
        }
    }
}