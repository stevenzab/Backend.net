namespace PlayerBack.Models
{
    public class StatisticsModel
    {
        public string? CountryCodeWithHighestWinRatio { get; set; }
        public double HighestWinRatio { get; set; }
        public double AverageBmi { get; set; }
        public double MedianHeight { get; set; }
    }
}
