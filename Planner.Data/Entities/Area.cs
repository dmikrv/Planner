using System;
using System.Collections.Generic;

namespace Planner.Data.Entities;

public partial class Area
{
        
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public Color? Color { get; set; }
    public string AccountId { get; set; } 

    public virtual Account Account { get; set; } = null!;
    public virtual ICollection<Action> Actions { get; set; }
}