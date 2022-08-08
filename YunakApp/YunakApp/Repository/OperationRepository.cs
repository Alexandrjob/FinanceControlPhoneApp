using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using YunakApp.Interface;
using YunakApp.Models;
using YunakApp.Services;

namespace YunakApp.Repository
{
    public class OperationRepository : IOperationRepository
    {
        private readonly  MockDataStore DataStore;

        public OperationRepository(MockDataStore store)
        {
             DataStore = store;
        }

        /// <summary>
        /// Получает список операций пользователя.
        /// </summary>
        /// <returns><see cref="IEnumerable{Category}"/><see cref="Category"/></returns>
        public async Task<IEnumerable<Operation>> GetOperationsAsync()
        {
            //await DataStore.GetOperationsDataAsync();
            var operations = await DataStore.InitializeOperationsAsync();
            //return await DataStore.GetOperationsDataAsync();
            return operations;
        }
    }
}