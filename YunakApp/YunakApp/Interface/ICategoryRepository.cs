using System.Collections.ObjectModel;
using System.Threading.Tasks;
using YunakApp.Models;

namespace YunakApp.Interface
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Получает список категорий.
        /// </summary>
        /// <returns><see cref="ObservableCollection{Category}"/><see cref="Category"/></returns>
        Task<ObservableCollection<Category>> GetCategoriesAsync();
    }
}
