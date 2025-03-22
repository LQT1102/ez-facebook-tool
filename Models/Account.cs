using System;

namespace EZ.FACEBOOK.TOOL.Models
{
    public class Account
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public string Status { get; set; } = "Sẵn sàng";
    }
} 