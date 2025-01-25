﻿namespace Logic.Interfaces.EmailAccountInterface.Models
{
    public class EmailAccountConnectionTestRequest
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
