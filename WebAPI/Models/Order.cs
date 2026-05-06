using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int TableId { get; set; }

    public int? CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public int? ReservationId { get; set; }

    public DateTime OrderTime { get; set; }

    public string Status { get; set; } = null!;

    public decimal SubTotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Note { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Reservation? Reservation { get; set; }

    public virtual TableEntity Table { get; set; } = null!;
}
