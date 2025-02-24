namespace ProleitPocBackend.Model
{
    public class DataFilter
    {
        public string? Machine { get; set; }
        public string? Property { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}
