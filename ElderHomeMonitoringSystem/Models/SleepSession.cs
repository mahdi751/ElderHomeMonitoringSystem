namespace ElderHomeMonitoringSystem.Models
{
    public class SleepSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
    }
}
