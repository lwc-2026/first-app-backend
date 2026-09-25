using System;

namespace DataAccess.Entities;

public interface IAuditable
{
    DateTime? CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}
