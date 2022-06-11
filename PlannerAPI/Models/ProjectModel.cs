namespace PlannerAPI.Models;

public class ProjectModel
{
    public long Id { get; set; }
    public enum ProjectStateModel
    {
        Active,
        Scheduled,
        Someday,
    }
}