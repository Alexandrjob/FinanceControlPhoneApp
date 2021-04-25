using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YunakApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenu: MasterDetailPage
    {
        public HamburgerMenu()
        {
            InitializeComponent();
            MyMenu();
        }
        public void MyMenu()
        {
            Detail = new NavigationPage(new MainPage());
            List<Menu> menu = new List<Menu>
            {
                new Menu{ Page= new Profile(),MenuTitle="Мой профиль"},
                new Menu{ Page= new MainPage(),MenuTitle="Главная страница"}
            };

            ListMenu.ItemsSource = menu;
        }
        private void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Menu;
            if (menu != null)
            {
                IsPresented = false;
                Detail = new NavigationPage(menu.Page);
            }
        }
    }

    class Menu
    {
        public Page Page { get; set; }
        public string MenuTitle { get; set; }
    }
}