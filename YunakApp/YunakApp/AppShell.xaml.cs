using Xamarin.Forms;
using YunakApp.Views;

namespace YunakApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
        }
    }
}