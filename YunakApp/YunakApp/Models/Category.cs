namespace YunakApp.Models
{
    /// <summary>
    /// Класс категории.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Итоговая затрата на категорию.
        /// </summary>
        public double TotalMoney { get; set; }
        /// <summary>
        /// Процент от общего заработка.
        /// </summary>
        public double PercentageTotalCosts { get; set; }
        /// <summary>
        /// Тип категории, доход/расход.
        /// </summary>
        public Type Type { get; set; }
    }
}
