namespace ElderHomeMonitoringSystem.Models
{
    public class SleepPosition
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Position { get; set; }
        public DateTime AtTime { get; set; }
    }
}
