using System;
using System.ComponentModel.DataAnnotations;
namespace DataAccess.Entities;

public abstract class BaseEntity
{
    public DateTime? CreatedAt { get; set; } = null;
    public DateTime? UpdatedAt { get; set; } = null;
}
