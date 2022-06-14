namespace Planner.Data.Entities
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
        public string? TimeRequired { get; set; } 
        public EnergyLevel? Energy { get; set; }
        public DateTime? DueDate { get; set; }
        public ActionState State { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountId { get; set; }
        public long? ProjectId { get; set; }
        
        public DateTime? ScheduledDate { get; set; }
        public long? WaitingContactId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Project? Project { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
        public virtual ICollection<Area>? Areas { get; set; }
        public virtual ICollection<Contact>? Contacts { get; set; }
        public virtual Contact? WaitingContact { get; set; }
        
        
        public enum ActionState
        {
            Inbox,
            Next,
            Waiting,
            Scheduled,
            Someday
        }
        
        public enum EnergyLevel
        {
            Low,
            Middle,
            High
        }
    }
}
