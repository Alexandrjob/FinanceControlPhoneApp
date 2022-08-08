namespace YunakApp.Models
{
    /// <summary>
    /// Информация пользователя.
    /// </summary>
    public class GeneralInformation
    {
        /// <summary>
        /// Месячная прибыль.
        /// </summary>
        public double MonthlyIncome { get; set; }
        /// <summary>
        /// Месячный расход.
        /// </summary>
        public double MonthlyConsumption { get; set; }
        private int percentageDifference;
        /// <summary>
        /// Процентный остаток.
        /// </summary>
        public int PercentageDifference { get => percentageDifference = (int)(MonthlyConsumption / (MonthlyIncome / 100)); }
        private int balance;
        /// <summary>
        /// Остаток(Высчитывается как заработок за месяц - разход за месяц).
        /// </summary>
        public int Balance { get => balance = (int)(MonthlyIncome - MonthlyConsumption); }
    }
}
