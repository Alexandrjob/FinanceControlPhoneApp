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
    /// <summary>
    /// Класс имитации базы данных.
    /// </summary>
    public class MockDataStore
    {
        private readonly string CATEGORIESFILEPATCH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "categories.json");
        private readonly string OPERATIONSFILEPATCH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "operations.json");
        private readonly string USERSFILEPATCH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "user.json");

        private List<Category> Categories;
        private List<Operation> Operations;
        private User User;

        public List<Operation> SortOperations { get; set; }

        public MockDataStore()
        {
            Categories = new List<Category>();
            Operations = new List<Operation>();
            User = new User();

            InitializeOperations();
            InitializeUser();
            InitializeCategories();
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

        private async Task<IEnumerable<Category>> GetCategoryDataAsync()
        {
            //Без операций не будет работать, а в других местах происходит рассинхрон.Костыль
            //await GetOperationsDataAsync();
            await CreateOrWriteFileIfEmpty(CATEGORIESFILEPATCH, InitializeCategoriesAsync);

            try
            {
                using (var fs = new FileStream(CATEGORIESFILEPATCH, FileMode.Open))
                {
                    return await JsonSerializer.DeserializeAsync<IEnumerable<Category>>(fs);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Ошибка чтения json файла  " + e.Message);
                return new List<Category>();
            }
        }

        private async Task<IEnumerable<Operation>> GetOperationsDataAsync()
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

        private async Task<User> GetUserDataAsync()
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

        public async Task<List<Category>> InitializeCategoriesAsync()
        {
            return await Task.Run(() => InitializeCategories());
        }

        public async Task<List<Operation>> InitializeOperationsAsync()
        {
            return await Task.Run(() => InitializeOperations());
        }

        public async Task<User> InitializeUserAsync()
        {
            return await Task.Run(() => InitializeUser());
        }

        private List<Category> InitializeCategories()
        {
            Random random = new Random();

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
            Category category4 = new Category()
            {
                Name = "Без категории",
                Type = Models.Type.consumption
            };

            var categoriesArray = new Category[4];
            categoriesArray[0] = category1;
            categoriesArray[1] = category2;
            categoriesArray[2] = category3;
            categoriesArray[3] = category4;

            //Высчитываю итоговую сумму на категорию.
            Categories = new List<Category>(categoriesArray);

            foreach (var item in Operations)
            {
                item.Category = categoriesArray[random.Next(0, 3)];
                var category = Categories.Where(c => c.Name == item.Category.Name).FirstOrDefault();
                if (category != null)
                {
                    Categories.Where(c => c.Name == item.Category.Name).FirstOrDefault().TotalMoney += item.Cost;
                    continue;
                }

                item.Category.TotalMoney += item.Cost;
                Categories.Add(item.Category);
            }

            foreach (var item in Categories)
            {
                item.PercentageTotalCosts = Math.Round(item.TotalMoney / (User.GeneralInformation.MonthlyConsumption / 100), 1);
            }

            return Categories;
        }

        private List<Operation> InitializeOperations()
        {
            Random random = new Random();

            Category category = new Category()
            {
                Name = "Без категории",
                Type = Models.Type.consumption
            };

            Operations = new List<Operation>();
            for (int i = 0; i < 38; i++)
            {
                Operation operation = new Operation()
                {
                    Cost = random.Next(100, 10000),
                    Date = DateTime.Now.AddDays(random.Next(-30, 30)),
                    Category = category
                };
                Operations.Add(operation);
            }

            //foreach (var item in Operations)
            //{
            //    item.PercentageTotalCostsInCategory = Math.Round(item.Cost / (item.Category.TotalMoney / 100), 2);
            //}

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

        public List<Category> GetCategories()
        {
            return Categories;
        }

        public List<Operation> GetOperations()
        {
            return Operations;
        }

        public User GetUser()
        {
            return User;

        }

        public async Task<List<Operation>> GetCategoryOperationsAsync(string NameCategory, Models.Type type)
        {
            return await Task.FromResult(Operations.Where(o => o.Category.Type == type && o.Category.Name == NameCategory).OrderByDescending(o => o.Cost).ToList());
        }
    }
}
