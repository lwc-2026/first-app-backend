using System;
using System.ComponentModel.DataAnnotations;
namespace DataAccess.Entities;

public class BaseEntity
{
    [Required]
    public DateTime CreatedAt { get; set; }
    [Required]
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } = null;
}
