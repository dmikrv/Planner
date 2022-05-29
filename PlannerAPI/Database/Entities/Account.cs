namespace PlannerAPI.Database.Entities
{
    public partial class Account
    {
        public Account()
        {
            Actions = new HashSet<Action>();
            Areas = new HashSet<Area>();
            Contacts = new HashSet<Contact>();
            Projects = new HashSet<Project>();
            Tags = new HashSet<Tag>();
            TrashActions = new HashSet<TrashAction>();
        }

        public long Id { get; set; }
        public string Email { get; set; } = null!;

        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<TrashAction> TrashActions { get; set; }
    }
}
