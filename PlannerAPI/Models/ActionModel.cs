namespace PlannerAPI.Models;

public abstract class ActionModel
{
    public long Id { get; set; }
    public string Text { get; set; }
    public string? Notes { get; set; }
    public ActionStates State { get; set; }
    public bool IsDone { get; set; }
    public bool IsFocused { get; set; }
    
    public TimeSpan? TimeSpan { get; set; }
    public ActionEnergies? Energy { get; set; }
    public DateTime? DueDate { get; set; }
    
    public IEnumerable<TagModel> Tags { get; set; }

    public long? ProjectId { get; set; }
    
    public TagModel? WaitingContact { get; set; } // only with state Waiting
    public DateTime? ScheduledTime { get; set; } // only with state Scheduled
}

