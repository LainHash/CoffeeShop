using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid PublicId { get; set; }

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? ConfirmationToken { get; set; }

    public DateTime? TokenExpiry { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
