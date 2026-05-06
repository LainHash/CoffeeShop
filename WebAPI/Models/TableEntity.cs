using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class TableEntity
{
    public int TableId { get; set; }

    public string TableCode { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public int AreaId { get; set; }

    public int Capacity { get; set; }

    public string Status { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
