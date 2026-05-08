using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public string Position { get; set; } = null!;

    public bool IsActive { get; set; }

    public Guid PublicId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
