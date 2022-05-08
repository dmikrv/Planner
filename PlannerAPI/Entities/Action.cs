﻿using System;
using System.Collections.Generic;

namespace PlannerAPI.Entities
{
    public partial class Action
    {
        public Action()
        {
            Tags = new HashSet<Tag>();
            Areas = new HashSet<Area>();
            Contacts = new HashSet<Contact>();
        }
        
        public long Id { get; set; }
        public string Text { get; set; } = null!;
        public string? Notes { get; set; }
        public bool IsDone { get; set; }
        public bool IsFocused { get; set; }
        public TimeSpan? TimeRequired { get; set; }
        public int? EnergyId { get; set; }
        public DateTime? DueDate { get; set; }
        public int StateId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long AccountId { get; set; }
        public long? ProjectId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Energy? Energy { get; set; }
        public virtual Project? Project { get; set; }
        public virtual ActionState State { get; set; } = null!;
        public virtual WaitingAction WaitingAction { get; set; } = null!;
        public virtual ICollection<Tag>? Tags { get; set; }
        public virtual ICollection<Area>? Areas { get; set; }
        public virtual ICollection<Contact>? Contacts { get; set; }
    }
}