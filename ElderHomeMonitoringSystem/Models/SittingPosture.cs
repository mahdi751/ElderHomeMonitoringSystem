using System.Text.Json.Serialization;

namespace ElderHomeMonitoringSystem.Models
{
    public class SittingPosture
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public bool GoodPosture { get; set; }
        public double PostureDuration{ get; set; }


        [JsonIgnore]
        public User? User { get; set; }
        public int UserID { get; set; }
    }
}
