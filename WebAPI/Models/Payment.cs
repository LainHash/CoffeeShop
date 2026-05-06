using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public DateTime PaymentTime { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal PaidAmount { get; set; }

    public string? Note { get; set; }

    public virtual Order Order { get; set; } = null!;
}
