using ElderHomeMonitoringSystem.Enums;
using System.Text.Json.Serialization;

namespace ElderHomeMonitoringSystem.Models
{
    public class Elder
    {
        public int ElderId{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string ElderCode { get; set; }
        public GenderType Gender { get; set; }
        public byte[] ProfileImage { get; set; }
        public string Address { get; set; }
        public string MacAddress { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        public int UserID { get; set; }
    }
}
