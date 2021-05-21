namespace YunakApp.Models
{
    public class GeneralInformation
    {
        public double MonthlyIncome { get; set; }
        public double MonthlyConsumption { get; set; }
        private int percentageDifference;
        public int PercentageDifference { get => percentageDifference = (int)(MonthlyConsumption / (MonthlyIncome / 100)); }
    }
}
