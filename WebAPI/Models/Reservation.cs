using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int CustomerId { get; set; }

    public int TableId { get; set; }

    public DateTime ReservationTime { get; set; }

    public int NumberOfGuests { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual TableEntity Table { get; set; } = null!;
}
