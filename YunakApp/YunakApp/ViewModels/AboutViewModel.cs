using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using YunakApp.Models;
using YunakApp.Views;

namespace YunakApp.ViewModels
{
    class AboutViewModel : BaseViewModel
    {
        #region Property
        public ObservableCollection<Category> Catigories { get; set; }

        public float progressBarVallue;
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

        bool isRefreshing;
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
        public Command<Category> ItemTapped { get; }
        #endregion

        public AboutViewModel()
        {
            Catigories = new ObservableCollection<Category>();
            UserGeneralInformation = new GeneralInformation();

            ItemTapped = new Command<Category>(OnItemSelected);
            LoadDataCommand = new Command(async () => await GetData());

            Task.Run(async () => await GetData());
        }

        private async Task GetData()
        {
            IsRefreshing = true;

            User user = await DataStore.GetUserDataAsync();
            UserGeneralInformation = user.GeneralInformation;

            Catigories.Clear();

            var categories = DataStore.Catigories;
            foreach (var item in categories)
            {
                Catigories.Add(item);
            }
           
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
            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}?name={nameCategory}&type={typeStr}");
        }
    }
}
