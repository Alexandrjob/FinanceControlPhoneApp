using System;

namespace YunakApp.Models
{
    /// <summary>
    /// Класс операций.
    /// </summary>
    public class Operation
    {
        public int Id { get; set; }
        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Цена.
        /// </summary>
        public double Cost { get; set; }
        /// <summary>
        /// Дата соверщения операции.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Процент операции от общей цены категории.
        /// </summary>
        public double PercentageTotalCostsInCategory { get; set; }
        /// <summary>
        /// Класс категории к тоторой принадлежит операция.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Иницилизация пустой категории.
        /// </summary>
        public Operation()
        {
            Category = new Category();
        }
    }
}
