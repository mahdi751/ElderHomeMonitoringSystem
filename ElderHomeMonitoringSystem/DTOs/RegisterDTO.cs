using System.ComponentModel.DataAnnotations;

namespace ElderHomeMonitoringSystem.DTOs
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] ProfileImage { get; set; }
        public string Disability { get; set; }
    }
}
