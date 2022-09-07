using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Получает список катигорий отсортированных по дате(Cортировка проходит над операциями но вытягиваются дынные только категорий).
        /// </summary>
        /// <param name="dateTimeStart">Дата начала диапозона.</param>
        /// <param name="dateTimeEnd">Дата конца диапозона.</param>
        ///  <returns><see cref="ObservableCollection{Category}"/><see cref="Category"/></returns>
        Task<ObservableCollection<Category>> GetCategoriesSortedByDateAsync(DateTime dateTimeStart, DateTime dateTimeEnd);

        /// <summary>
        /// Получает список катигорий отсортированных по типу и дате(Cортировка проходит над операциями но вытягиваются дынные только категорий).
        /// </summary>
        /// <param name="type">Тип категории(доход/расход).</param>
        /// <param name="dateTimeStart">Дата начала диапозона.</param>
        /// <param name="dateTimeEnd">Дата конца диапозона.</param>
        /// <returns><see cref="List{Category}"/><see cref="Category"/></returns>
        Task<List<Category>> GetCategoriesSortedByDateAsync(Models.Type type, DateTime dateTimeStart, DateTime dateTimeEnd);

        /// <summary>
        /// Добавляет категорию.
        /// </summary>
        /// <param name="name">Название категории.</param>
        /// <param name="type">Тип категории(доход/расход).</param>
        /// <returns></returns>
        Task AddCategoryAsync(string name, Models.Type type);

        /// <summary>
        /// Редактирует категорию.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <returns></returns>
        Task EditCategoryAsync(Category category);

        /// <summary>
        /// Удаляет категорию.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <returns></returns>
        Task DeleteAsync(Category category);
    }
}