namespace FoosballXamarin.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new FoosballXamarin.App());
        }
    }
}