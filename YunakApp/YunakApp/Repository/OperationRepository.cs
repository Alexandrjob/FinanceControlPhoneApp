using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public async Task<List<Operation>> GetOperationsAsync()
        {
            //await DataStore.GetOperationsDataAsync();
            var operations = DataStore.GetOperations();
            //return await DataStore.GetOperationsDataAsync();
            return operations;
        }

        public async Task<List<Operation>> GetOperationsSortedByDateAsync(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            var sortOperations = DataStore.GetOperations().Where(o => o.Date < dateTimeEnd & o.Date > dateTimeStart).ToList();

            return sortOperations;
        }


        public async Task<List<Operation>> GetCategoryOperationsSortedByDateAsync(string categoryName, Models.Type type, DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            //Выражение состоит из двух частей. 
            //Первая часть это список операций остсортированных по периоду указанных дат.
            //Вторая часть это список операций отсортированный по названию категории и ее типу.
            var operations = DataStore.GetOperations().Where(o => o.Date < dateTimeEnd & o.Date > dateTimeStart)
                                                          .Where(o => o.Category.Type == type && o.Category.Name == categoryName)
                                                          .OrderByDescending(o => o.Cost)
                                                          .ToList();

            return operations;
        }

        public async Task AddOperationAsync(string nameCategory, string nameOperation, int cost, DateTime date)
        {
            await DataStore.AddOperationAsync(nameCategory, nameOperation, cost, date);
        }
    }
}