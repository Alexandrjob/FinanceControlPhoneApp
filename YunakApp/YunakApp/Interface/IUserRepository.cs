using System.Threading.Tasks;
using YunakApp.Models;

namespace YunakApp.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// Получает Информацию о доходах и расходах пользователя.
        /// </summary>
        /// <returns><see cref="GeneralInformation"/></returns>
        Task<GeneralInformation> GetGeneralInformation();
    }
}
