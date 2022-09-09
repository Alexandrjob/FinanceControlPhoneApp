using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<Category>> GetCategoriesSortedByDateAsync(Models.Type type, DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            var sortOperations = DataStore.GetOperations().Where(o => o.Date < dateTimeEnd & o.Date > dateTimeStart).ToList();

            List<Category> sortCategories = new List<Category>();

            foreach (var item in sortOperations)
            {
                //TODO: Получается так что с одним названием и типом категория не будет отображаться.
                if (!sortCategories.Any(c => c.Name == item.Category.Name))
                {
                    sortCategories.Add(item.Category);
                }
                ///TODO: Думаю неэфективный это код.
                if (sortCategories.Any(c => c.Type != type))
                {
                    sortCategories.Remove(item.Category);
                }

            }

            return sortCategories.OrderByDescending(c => c.TotalMoney).ToList();
        }

        public async Task AddCategoryAsync(string name, Models.Type type)
        {
            await DataStore.AddCategoryAsync(name, type);

            ///TODO: Необхожимые действия для того чтобы созданная категория отображалась в списке(Причина в том что список категорий получается путем сортировки операций).
            await DataStore.AddOperationAsync(name, "Default", 0, DateTime.Now);
        }

        public async Task DeleteAsync(Category category)
        {
            await DataStore.DeleteCategoryAsync(category);
        }

        public async Task EditCategoryAsync(Category category)
        {
            await DataStore.EditCategoryAsync(category);
        }
    }
}