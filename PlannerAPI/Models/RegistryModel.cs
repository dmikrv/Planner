using System.ComponentModel.DataAnnotations;

namespace PlannerAPI.Models;

public class RegistryModel
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}