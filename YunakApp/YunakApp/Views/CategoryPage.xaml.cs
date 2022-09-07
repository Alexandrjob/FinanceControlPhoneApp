using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YunakApp.ViewModels;

namespace YunakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage()
        {
            InitializeComponent();

            BindingContext = new CategoryViewModel(LabelType, GridLabelBalance);
        }
    }
}