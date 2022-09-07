using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunakApp.Models;

namespace YunakApp.Interface
{
    public interface IOperationRepository
    {
        /// <summary>
        /// Получает список операций пользователя.
        /// </summary>
        /// <returns><see cref="List{Category}"/><see cref="Category"/></returns>
        Task<List<Operation>> GetOperationsAsync();

        /// <summary>
        /// Получает список операций отсортированные по диапозону дат.
        /// </summary>
        /// <param name="dateStart">Дата начала диапозона.</param>
        /// <param name="dateEnd">Дата конца диапозона.</param>
        /// <returns><see cref="List{Operation}"/><see cref="Operation"/></returns>
        Task<List<Operation>> GetOperationsSortedByDateAsync(DateTime dateTimeStart, DateTime dateTimeEnd);

        /// <summary>
        /// Получает операции выбранной категории отсортированной по дате.
        /// </summary>
        /// <param name="categoryName">Название категории.</param>
        /// <param name="type">Тип категории(Доход/расход).</param>
        /// <param name="dateTimeStart">Дата начала диапозона.</param>
        /// <param name="dateTimeEnd">Дата конца диапозона.</param>
        /// <returns><see cref="List{Operation}"/><see cref="Operation"/></returns>
        Task<List<Operation>> GetCategoryOperationsSortedByDateAsync(string categoryName, Models.Type type, DateTime dateTimeStart, DateTime dateTimeEnd);

        /// <summary>
        /// Создает операцию в категории.
        /// </summary>
        ///  /// <param name="nameCategory">Название категории к которой принадлежит операция.</param>
        /// <param name="nameOperation">Название операции.</param>
        /// <param name="cost">Цена.</param>
        /// <param name="date">Дата соверщения операции.</param>
        /// <returns></returns>
        Task AddOperationAsync(string nameCategory, string nameOperation, int cost, DateTime date);
        Task DeleteAsync(Operation operation);
        Task EditOperationAsync(Operation operation);
    }
}
