﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Planner.Auth.Common;

public class JwtAuthOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public int TokenLifetime { get; set; } // in seconds

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
    
}