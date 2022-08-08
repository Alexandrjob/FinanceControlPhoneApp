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
        /// <returns><see cref="IEnumerable{Category}"/><see cref="Category"/></returns>
        Task<IEnumerable<Operation>> GetOperationsAsync();
    }
}
