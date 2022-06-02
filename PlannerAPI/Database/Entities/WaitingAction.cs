using System;
using System.Collections.Generic;

namespace PlannerAPI.Database.Entities
{
    public partial class WaitingAction
    {
        public long ActionId { get; set; }
        public long ContactId { get; set; }

        public virtual Action Action { get; set; } = null!;
        public virtual Contact Contact { get; set; } = null!;
    }
}
