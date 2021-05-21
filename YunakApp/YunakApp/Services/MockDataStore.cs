using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using YunakApp.Models;
using System.Threading.Tasks;
using System.Diagnostics;

namespace YunakApp.Services
{
    public class MockDataStore
    {
        private readonly string OPERATIONSFILEPATCH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "operations.json");
        private readonly string USERSFILEPATCH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "user.json");
        private User User;

        private async Task CreateOrWriteFileIfEmpty<T>(string filePatch, Func<T> func)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            using (var fs = new FileStream(filePatch, FileMode.Create))
            {

                var data =  func();
                await JsonSerializer.SerializeAsync(fs, data, options);

            }
        }

        public async Task<IEnumerable<Operation>> GetOperationsDataAsync()
        {
            await CreateOrWriteFileIfEmpty(OPERATIONSFILEPATCH, InitializeOperations);

            try
            {
                using (var fs = new FileStream(OPERATIONSFILEPATCH, FileMode.Open))
                {
                    return await JsonSerializer.DeserializeAsync<IEnumerable<Operation>>(fs);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Ошибка чтения json файла  " + e.Message);
                return new List<Operation>();
            }
        }

        public async Task<User> GetUserDataAsync()
        {
            await CreateOrWriteFileIfEmpty(USERSFILEPATCH, InitializeUser);

            try
            {
                using (var fs = new FileStream(USERSFILEPATCH, FileMode.Open))
                {
                    return await JsonSerializer.DeserializeAsync<User>(fs);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Ошибка чтения json файла  " + e.Message);
                return new User();
            }
        }

        private async Task<List<Operation>> InitializeOperationsAsync()
        {
            return await Task.Run(() => InitializeOperations());
        }

        private async Task<User> InitializeUserAsync()
        {
            return await Task.Run(() => InitializeUser());
        }

        public List<Operation> InitializeOperations()
        {
            Random random = new Random();

            List<Operation> operations = new List<Operation>();

            for (int i = 0; i < 18; i++)
            {
                Category category = new Category()
                {
                    Name = "Транспорт" + i,
                    Type = Models.Type.income + random.Next(0, 2)
                };

                Operation operation = new Operation()
                {
                    Cost = random.Next(10, 1000),
                    Date = DateTime.Now,
                    Category = category
                };
                operation.PercentageTotalCosts = Math.Round(operation.Cost * (100 / User.GeneralInformation.MonthlyConsumption), 2);

                operations.Add(operation);
            }
            return operations;
        }

        public User InitializeUser()
        {
            Random random = new Random();

            GeneralInformation generalInformation = new GeneralInformation()
            {
                MonthlyConsumption = random.Next(100000, 200000),
                MonthlyIncome = random.Next(200000, 300000)
            };

            User user = new User()
            {
                FirstName = "Саша",
                LastName = "Саварский",
                Mail = "zaebymba@gmail.com",
                GeneralInformation = generalInformation
            };
            User = user;

            return user;
        }

    }
}
