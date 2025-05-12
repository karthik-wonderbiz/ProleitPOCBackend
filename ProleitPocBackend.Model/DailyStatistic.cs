namespace ProleitPocBackend.Model
{
    public class AggregateValue
    {
        public DateTime Date { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal AvgValue { get; set; }
    }
}
