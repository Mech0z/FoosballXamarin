using FoosballXamarin.ViewModels;
using Xamarin.Forms.Xaml;

namespace FoosballXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerDetailsPage
    {
        PlayerDetailsViewModel viewModel;
        public PlayerDetailsPage (PlayerDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }
    }
}