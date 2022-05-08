using System;
using System.Collections.Generic;

namespace PlannerAPI.Entities
{
    public partial class Contact
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ColorId { get; set; }
        public long AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Color? Color { get; set; }
    }
}
