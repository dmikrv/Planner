namespace PlannerAPI.Models;

public abstract class TagModel
{
    public long Id { get; set; }
    public Type Type { get; set; }
    public string Name { get; set; }
    public Color? Color { get; set; }
}

public enum Type
{
    LABEL,
    AREA,
    CONTACT
}

public enum Color
{
    RED,
    GREEN,
}