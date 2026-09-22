using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Requests;

public class LoginHttpRequest
{
    [Required]
    public required string Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
