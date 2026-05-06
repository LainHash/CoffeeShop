using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Area
{
    public int AreaId { get; set; }

    public string AreaName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<TableEntity> TableEntities { get; set; } = new List<TableEntity>();
}
