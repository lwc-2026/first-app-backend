using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Requests;

public class ReturnAssetHttpRequest
{
    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }
}
