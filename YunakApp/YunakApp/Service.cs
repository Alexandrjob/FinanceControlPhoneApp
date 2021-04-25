using System;
using System.Net.Http;
using System.Threading.Tasks;
using YunakApp.Models;

namespace YunakApp
{
    class Service
    {
        const string Url = "https://";

        public Service()
        {
            InitializeObjects();
        }

        public void InitializeObjects()
        {
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var value = random.Next(0, 10);

                Сategory category = new Сategory
                {
                    Name = "Транспорт" + value,
                    Type = Models.Type.income + random.Next(0, 1)
                };

                Operation operation = new Operation
                {
                    Cost = random.Next(10, 1000),
                    Date = DateTime.Now,
                    Category = category
                };

                GeneralInformation generalInformation = new GeneralInformation
                {
                    MonthlyConsumption = random.Next(10000, 200000),
                    MonthlyIncome = random.Next(10000, 300000)
                };

                User user = new User
                {
                    FirstName = "Саша" + value,
                    LastName = "Саварский" + value,
                    Mail = "zaebymba@gmail.com" + value,
                    GeneralInformation = generalInformation
                };

                user.Operations.Add(operation);
            }
        }

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<string> GetCurrentUser(int Id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + Id);
            return result;
        }
    }
}
