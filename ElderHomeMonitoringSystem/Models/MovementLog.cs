namespace ElderHomeMonitoringSystem.Models
{
    public class MovementLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int MovementCount { get; set; }
        public string PostureWhenMoving { get; set; }

    }
}
