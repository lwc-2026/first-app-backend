using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Requests
{
    public class DeleteUserServiceRequest
    {
        public string Id { get; set; }
        
        public DeleteUserServiceRequest(string id)
        {
            Id = id;
        }
    }
}
