namespace Planner.Web.Api.Models;

public class ProjectModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Notes { get; set; }
    public DateTime? DueDate { get; set; }
    public ProjectStateModel State { get; set; }
    
    public enum ProjectStateModel
    {
        Active,
        // Scheduled,
        Someday,
    }
}