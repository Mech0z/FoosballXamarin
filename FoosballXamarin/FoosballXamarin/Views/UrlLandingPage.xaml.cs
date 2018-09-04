using FoosballXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UrlLandingPage
    {
        public UrlLandingPage()
        {
            InitializeComponent();

            BindingContext = new UrlLandingViewModel();

            MessagingCenter.Subscribe<UrlLandingViewModel>(this, "ApiPingSuccess",
                sender => { ((App)Application.Current).MainPage = new MainPage(); });
        }
    }
}