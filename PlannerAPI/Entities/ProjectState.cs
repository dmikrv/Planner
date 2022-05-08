using System;
using System.Collections.Generic;

namespace PlannerAPI.Entities
{
    public partial class ProjectState
    {
        public ProjectState()
        {
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
