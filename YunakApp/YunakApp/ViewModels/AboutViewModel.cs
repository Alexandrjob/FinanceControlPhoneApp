using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using YunakApp.Models;
using YunakApp.Views;

namespace YunakApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        #region Property
        readonly EnterPeroidPopupPage EnterPeroidPopupPage;

        private Button Button;
        private Type type;
        public ObservableCollection<Category> Categories { get; set; }

        private float progressBarVallue;
        public float ProgressBarVallue
        {
            get => progressBarVallue;
            set
            {
                if (value > 1)
                {
                    progressBarVallue = 1;
                    return;
                }
                else if (value < 0)
                {
                    progressBarVallue = 0;
                    return;
                }

                progressBarVallue = value;
                OnPropertyChanged(nameof(ProgressBarVallue));
            }
        }

        private GeneralInformation userGeneralInformation;
        public GeneralInformation UserGeneralInformation
        {
            get => userGeneralInformation;
            set
            {
                userGeneralInformation = value;
                OnPropertyChanged(nameof(UserGeneralInformation));
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public Command LoadDataCommand { get; }
        public Command<Category> ItemTappedCommand { get; }
        public Command SwapCommand { get; }
        public Command AddCategoryCommand { get; }
        public Command SortByDateTimecommand { get; }
        #endregion

        public AboutViewModel(Button button)
        {
            EnterPeroidPopupPage = new EnterPeroidPopupPage(this);

            Button = button;

            Categories = new ObservableCollection<Category>();
            UserGeneralInformation = new GeneralInformation();

            ItemTappedCommand = new Command<Category>(OnItemSelected);
            LoadDataCommand = new Command(async () => await GetData());
            SwapCommand = new Command(OnButtonClickedSwapIncomeConsumption);
            AddCategoryCommand = new Command(OnButtonClickedAddCategory);
            SortByDateTimecommand = new Command(OnButtonSortByDateTimeAsync);
            Task.Run(async () => await GetData());
        }

        private async Task GetData()
        {
            IsRefreshing = true;

            UserGeneralInformation = await _userRepository.GetGeneralInformation();
            Categories.Clear();

            Categories = await _categoryRepository.GetCategoriesSortedByDateAsync(EnterPeroidPopupPage.DateTimeStart, EnterPeroidPopupPage.DateTimeEnd);
            OnPropertyChanged(nameof(Categories));

            IsRefreshing = false;
            await SetValueProgressBarAsync();
        }

        private async Task SetValueProgressBarAsync()
        {
            await Task.Run(async () => await SetValueProgressBar());
        }

        private Task SetValueProgressBar()
        {
            for (int i = 0; i < 170; i++)
            {
                ProgressBarVallue -= 0.006f;
                Thread.Sleep(1);
            }

            var percentageDifferenceFloat = (double)(UserGeneralInformation.MonthlyConsumption / (UserGeneralInformation.MonthlyIncome / 100) / 100);
            for (int i = 0; i < 250; i++)
            {
                if (ProgressBarVallue + 0.004f <= percentageDifferenceFloat)
                {
                    ProgressBarVallue += 0.004f;
                    Thread.Sleep(1);
                }
                else
                {
                    break;
                }
            }

            return Task.CompletedTask;
        }

        private async void OnItemSelected(Category category)
        {
            if (category == null)
                return;
            var nameCategory = category.Name;
            var typeStr = ((int)category.Type).ToString();
            //Берет данные либо дефолтные(начало и конец месяца) либо измененные в окне внесения периода.
            var dateTimeStartSort = EnterPeroidPopupPage.DateTimeStart;
            var dateTimeEndSort = EnterPeroidPopupPage.DateTimeEnd;

            var result = $"{nameof(CategoryPage)}?name={nameCategory}&type={typeStr}&dateTimeSs={dateTimeStartSort.ToShortDateString()}&dateTimeEs={dateTimeEndSort.ToShortDateString()}";
            await Shell.Current.GoToAsync(result);
        }

        private async void OnButtonClickedSwapIncomeConsumption()
        {
            if (Button.Text == "Доходы")
            {
                Button.Text = "Расходы";
                type = Type.consumption;

                Categories.Clear();

                Categories = await _categoryRepository.GetCategoriesSortedByDateAsync(type, EnterPeroidPopupPage.DateTimeStart, EnterPeroidPopupPage.DateTimeEnd);
                OnPropertyChanged(nameof(Categories));
                return;
            }

            type = Type.income;
            Categories.Clear();

            Categories = await _categoryRepository.GetCategoriesSortedByDateAsync(type, EnterPeroidPopupPage.DateTimeStart, EnterPeroidPopupPage.DateTimeEnd);
            OnPropertyChanged(nameof(Categories));
            Button.Text = "Доходы";
        }

        private async void OnButtonClickedAddCategory(object parameter)
        {
            var page = new AddCategoryPopupPage(_categoryRepository);

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnButtonSortByDateTimeAsync(object obj)
        {

            await PopupNavigation.Instance.PushAsync(EnterPeroidPopupPage);
        }

        public async Task SortByDateTimeAsync()
        {
            var dateTimeStart = EnterPeroidPopupPage.DateTimeStart.Date;
            var dateTimeEnd = EnterPeroidPopupPage.DateTimeEnd.Date;   

            Categories.Clear();

            Categories = await _categoryRepository.GetCategoriesSortedByDateAsync(dateTimeStart, dateTimeEnd);
            OnPropertyChanged(nameof(Categories));
        }
    }
}
