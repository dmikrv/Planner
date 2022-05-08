using System;
using System.Collections.Generic;

namespace PlannerAPI.Entities
{
    public partial class Project
    {
        public Project()
        {
            Actions = new HashSet<Action>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime? DueDate { get; set; }
        public int StateId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ProjectState State { get; set; } = null!;
        public virtual ICollection<Action> Actions { get; set; }
    }
}
