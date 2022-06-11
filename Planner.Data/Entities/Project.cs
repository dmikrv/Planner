using System;
using System.Collections.Generic;

namespace Planner.Data.Entities
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
        public ProjectState State { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Action> Actions { get; set; }
        
        public enum ProjectState
        {
            Active,
            // Scheduled,
            Someday,
        }
    }
}
