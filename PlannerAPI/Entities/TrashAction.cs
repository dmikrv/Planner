using System;
using System.Collections.Generic;

namespace PlannerAPI.Entities
{
    public partial class TrashAction
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
