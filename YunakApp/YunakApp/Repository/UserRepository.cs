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

        public async Task<GeneralInformation> GetGeneralInformation()
        {
            var user = DataStore.GetUser();
            //User user = await DataStore.GetUserDataAsync();
            return user.GeneralInformation;
        }
    }
}
