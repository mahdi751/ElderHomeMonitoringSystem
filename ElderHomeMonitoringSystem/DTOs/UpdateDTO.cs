namespace ElderHomeMonitoringSystem.DTOs
{
    public class UpdateDTO
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public byte[] profileImage { get; set; }
    }
}