using System;
using System.Collections.Generic;
using System.Text;
using BusinessModel.Enums;

namespace BusinessModel.DTOs
{ 
    public class UserDto
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public string? Email { get; set; }
    }
}