using System;
using System.Collections.Generic;

namespace PlannerAPI.Entities
{
    public partial class Color
    {
        public Color()
        {
            Areas = new HashSet<Area>();
            Contacts = new HashSet<Contact>();
            Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
