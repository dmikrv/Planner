namespace PlannerAPI.Models;

public abstract class TagModel
{
    public long Id { get; set; }
    public TagTypes Type { get; set; }
    public string Name { get; set; }
    public Colors? Color { get; set; }
}