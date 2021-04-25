using Xamarin.Forms;

namespace YunakApp
{
    public partial class App: Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            Service service = new Service();
            service.InitializeObjects();

            MainPage = new HamburgerMenu();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
