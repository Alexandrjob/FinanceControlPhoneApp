using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using YunakApp.Interface;
using YunakApp.Models;
using YunakApp.Services;

namespace YunakApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MockDataStore DataStore;

        public CategoryRepository(MockDataStore store)
        {
            DataStore = store;
        }

        public async Task<ObservableCollection<Category>> GetCategoriesAsync()
        {
            //TODO: Для дефолтного получения указать период дат как начало  и конец текущего месяца.
            ObservableCollection<Category> resultCategories = new ObservableCollection<Category>();
            //var categories = await DataStore.GetCategoryDataAsync();
            var categories = DataStore.GetCategories();

            foreach (var item in categories)
            {
                resultCategories.Add(item);
            }

            return resultCategories;
        }

        public async Task<ObservableCollection<Category>> GetCategoriesSortedByDateAsync(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            var sortOperations = DataStore.GetOperations().Where(o => o.Date < dateTimeEnd & o.Date > dateTimeStart).ToList();

            ObservableCollection<Category> categories = new ObservableCollection<Category>();

            foreach (var item in sortOperations)
            {
                if (!categories.Any(c => c.Name == item.Category.Name))
                {
                    categories.Add(item.Category);
                }
            }

            return categories;
        }

        public async Task AddCategoryAsync(string name, Models.Type type)
        {
            await DataStore.AddCategoryAsync(name, type);
        }
    }

}
