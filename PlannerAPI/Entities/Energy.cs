using System;
using System.Collections.Generic;

namespace PlannerAPI.Entities
{
    public partial class Energy
    {
        public Energy()
        {
            Actions = new HashSet<Action>();
        }

        public int Id { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
    }
}
