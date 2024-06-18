using System.Text.Json.Serialization;

namespace ElderHomeMonitoringSystem.Models
{
    public class MovementLog
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int MovementCount { get; set; }
        public string? PostureWhenMoving { get; set; }


        [JsonIgnore]
        public User? User { get; set; }
        public int UserID { get; set; }

    }
}
