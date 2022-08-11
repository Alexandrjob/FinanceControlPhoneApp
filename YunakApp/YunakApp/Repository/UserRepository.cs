using System.Threading.Tasks;
using YunakApp.Interface;
using YunakApp.Models;
using YunakApp.Services;

namespace YunakApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MockDataStore DataStore;

        public UserRepository(MockDataStore store)
        {
            DataStore = store;
        }

        /// <summary>
        /// Получает Информацию о доходах и расходах пользователя.
        /// </summary>
        /// <returns><see cref="GeneralInformation"/></returns>
        public async Task<GeneralInformation> GetGeneralInformation()
        {
            var user = DataStore.GetUser();
            //User user = await DataStore.GetUserDataAsync();
            return user.GeneralInformation;
        }
    }
}
