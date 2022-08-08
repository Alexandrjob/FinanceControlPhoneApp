using System.Collections.Generic;

namespace YunakApp.Models
{
    /// <summary>
    /// Класс Пользователя.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id пользователя в базе данных.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Почта пользователся.
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// Класс информации пользователя о доходах и расходах.
        /// </summary>
        public GeneralInformation GeneralInformation { get; set; }
        /// <summary>
        /// Список операций пользователя.
        /// </summary>
        public ICollection<Operation> Operations { get; set; }

        /// <summary>
        /// Инициализация пустого списка операций.
        /// </summary>
        public User()
        {
            Operations = new List<Operation>();
        }
    }
}
