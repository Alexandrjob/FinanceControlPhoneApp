using System;

namespace YunakApp.Models
{
    public class Operation
    {
        public double Cost { get; set; }
        public DateTime Date { get; set; }
        public double PercentageTotalCosts { get; set; }
        public Category Category { get; set; }
    }
}
