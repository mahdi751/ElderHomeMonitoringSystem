﻿namespace ElderHomeMonitoringSystem.DTOs
{
    public class ResponseDTO
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Activated { get; set; }
        public bool isAdmin { get; set; }
    }
}
