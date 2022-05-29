using System;
using System.Collections.Generic;

namespace PlannerAPI.Database.Entities
{
    public partial class ScheduledAction
    {
        public long ActionId { get; set; }
        public DateTime? Date { get; set; }
    }
}
