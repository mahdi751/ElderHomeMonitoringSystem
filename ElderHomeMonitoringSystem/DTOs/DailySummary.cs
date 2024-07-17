namespace ElderHomeMonitoringSystem.DTOs
{
    public class DailySummary
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public double GoodPostureDuration { get; set; }
        public double BadPostureDuration { get; set; }
    }
}
