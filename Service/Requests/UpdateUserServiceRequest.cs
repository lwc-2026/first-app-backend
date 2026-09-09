using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Requests
{
    public class UpdateUserServiceRequest
    {
        public required string Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
