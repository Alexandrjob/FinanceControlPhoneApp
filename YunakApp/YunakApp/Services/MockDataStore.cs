using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using YunakApp.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace YunakApp.Services
{
    public class MockDataStore
    {
        private readonly string OPERATIONSFILEPATCH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "operations.json");
        private readonly string USERSFILEPATCH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "user.json");

        private User User;
        private List<Operation> Operations;
        public List<Category> Catigories { get; set; }

        public MockDataStore()
        {
            Task.Run(async () => await GetOperationsDataAsync());
        }
        private async Task CreateOrWriteFileIfEmpty<T>(string filePatch, Func<Task<T>> func)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            using (var fs = new FileStream(filePatch, FileMode.Create))
            {

                var data = await func();
                await JsonSerializer.SerializeAsync(fs, data, options);

            }
        }

        public async Task<IEnumerable<Operation>> GetOperationsDataAsync()
        {
            await CreateOrWriteFileIfEmpty(OPERATIONSFILEPATCH, InitializeOperationsAsync);

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
            await CreateOrWriteFileIfEmpty(USERSFILEPATCH, InitializeUserAsync);

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

        private List<Operation> InitializeOperations()
        {
            Random random = new Random();

            Operations = new List<Operation>();

            Category category1 = new Category()
            {
                Name = "Транспорт",
                Type = Models.Type.consumption
            };
            Category category2 = new Category()
            {
                Name = "Еда",
                Type = Models.Type.consumption
            };
            Category category3 = new Category()
            {
                Name = "Ресторан",
                Type = Models.Type.consumption
            };

            var categories = new Category[3];
            categories[0] = category1;
            categories[1] = category2;
            categories[2] = category3;

            for (int i = 0; i < 18; i++)
            {
                Operation operation = new Operation()
                {
                    Cost = random.Next(100, 10000),
                    Date = DateTime.Now,
                    Category = categories[random.Next(0, 3)]
                };
                Operations.Add(operation);
            }

            Catigories = new List<Category>(categories);
            foreach (var item in Operations)
            {
                var category = Catigories.Where(c => c.Name == item.Category.Name).FirstOrDefault();
                if (category != null)
                {
                    category.Cost += item.Cost;
                    continue;
                }

                Catigories.Add(item.Category);
                item.Category.Cost += item.Cost;
            }

            foreach (var item in Catigories)
            {
                item.PercentageTotalCosts = Math.Round(item.Cost / (User.GeneralInformation.MonthlyConsumption / 100), 1);
            }

            foreach (var item in Operations)
            {
                item.PercentageTotalCostsInCategory = Math.Round(item.Cost / (item.Category.Cost / 100), 2);
            }

            return Operations;
        }

        private User InitializeUser()
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

        public async Task<List<Operation>> GetCategoryOperations(string NameCategory, Models.Type type)
        {
            return await Task.FromResult(Operations.Where(o => o.Category.Type == type && o.Category.Name == NameCategory).OrderByDescending(o => o.Cost).ToList());
        }
    }
}
