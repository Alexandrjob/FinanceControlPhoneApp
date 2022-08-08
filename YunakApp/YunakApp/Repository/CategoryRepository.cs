using System.Collections.ObjectModel;
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

        /// <summary>
        /// Получает список категорий.
        /// </summary>
        /// <returns><see cref="ObservableCollection{Category}"/><see cref="Category"/></returns>
        public async Task<ObservableCollection<Category>> GetCategoriesAsync()
        {
            ObservableCollection<Category> Categories = new ObservableCollection<Category>();
            //var categories = await DataStore.GetCategoryDataAsync();
            var categories = await DataStore.InitializeCategoriesAsync();

            foreach (var item in categories)
            {
                Categories.Add(item);
            }

            return Categories;
        }
    }

}
