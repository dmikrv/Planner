namespace PlannerAPI.Models;

public class ProjectModel
{
    public long id { get; set; }
    public enum ProjectStateModel
    {
        Active,
        Scheduled,
        Someday,
    }
}