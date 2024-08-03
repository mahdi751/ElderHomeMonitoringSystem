namespace ElderHomeMonitoringSystem.DTOs
{
    public class ResetPasswordDTO
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
