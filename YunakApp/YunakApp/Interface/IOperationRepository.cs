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
        /// <returns><see cref="List{Category}"/><see cref="Category"/></returns>
        Task<List<Operation>> GetOperationsSortedByDateAsync(DateTime dateStart, DateTime dateEnd);
    }
}
