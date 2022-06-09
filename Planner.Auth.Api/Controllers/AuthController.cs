using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using Planner.Auth.Api.Models;
using Planner.Auth.Common;
using Planner.Data;
using Planner.Data.Entities;
using Action = Planner.Data.Entities.Action;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Planner.Auth.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly PlannerContext _db;
    private readonly UserManager<Account> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<JwtAuthOptions> _authOptions;

    public AuthController(ILogger<AuthController> logger, PlannerContext db, UserManager<Account> userManager, 
        RoleManager<IdentityRole> roleManager, IOptions<JwtAuthOptions> authOptions)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _authOptions = authOptions;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> LoginAsync(LoginModel model)
    {
        var account = await _userManager.FindByNameAsync(model.Email);

        if (account is null)
            return Unauthorized();

        if (!await _userManager.CheckPasswordAsync(account, model.Password))
            return Unauthorized();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.UserName),      
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(JwtRegisteredClaimNames.Sub, account.Id),
        };
        
        var userRoles = await _userManager.GetRolesAsync(account);
        foreach (var userRole in userRoles)
            claims.Add(new Claim(ClaimTypes.Role, userRole));

        var authParams = _authOptions.Value;
        var token = new JwtSecurityToken(
            issuer: authParams.Issuer,
            audience: authParams.Audience,
            expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
            claims: claims,
            signingCredentials: new SigningCredentials(authParams.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
        
        return Ok(new {access_token = new JwtSecurityTokenHandler().WriteToken(token)});
    }
}