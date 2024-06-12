using System.Text.Json.Serialization;

namespace ElderHomeMonitoringSystem.Models
{
    public class ElderCares
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Elder? Elder { get; set; }
        public int ElderId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}
