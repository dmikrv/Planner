using System.ComponentModel.DataAnnotations;

namespace Planner.Web.Api.Models;

public class RegisterModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}