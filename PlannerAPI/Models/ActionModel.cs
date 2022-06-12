namespace PlannerAPI.Models;

public class ActionModel
{
    public long Id { get; set; }
    public string Text { get; set; }
    public string? Notes { get; set; }
    public ActionStateModel State { get; set; }
    public bool IsDone { get; set; }
    public bool IsFocused { get; set; }
    
    public int? TimeRequired { get; set; } // time in minutes 
    public EnergyLevelModel? Energy { get; set; }
    public DateTime? DueDate { get; set; }
    
    public IEnumerable<ContactTagModel>? ContactTags { get; set; }
    public IEnumerable<AreaTagModel>? AreaTags { get; set; }
    public IEnumerable<LabelTagModel>? LabelTags { get; set; }

    public long? ProjectId { get; set; }
    
    public ContactTagModel? WaitingContact { get; set; } // only with state Waitinfsdf
                                                         // sdf
                                                         // sdf
                                                         // g
    public DateTime? ScheduledDate { get; set; } // only with state Scheduled
    
    public enum ActionStateModel
    {
        Inbox,
        Next,
        Waiting,
        Scheduled,
        Someday
    }
        
    public enum EnergyLevelModel
    {
        Low,
        Middle,
        High
    }
}

