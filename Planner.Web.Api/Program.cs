using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Planner.Auth.Common;
using Planner.Data;
using Planner.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var connectionString = builder.Configuration.GetConnectionString("MSSQL");
// builder.Services.AddDbContext<PlannerContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDbContext<PlannerContext>(options => options.UseSqlServer(connectionString))
    .AddIdentityCore<Account>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 4;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PlannerContext>();

builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

var authOptions = builder.Configuration.GetSection("Auth").Get<JwtAuthOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
    // options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = authOptions.Audience,
        ValidIssuer = authOptions.Issuer,
        IssuerSigningKey = authOptions.GetSymmetricSecurityKey()
    };
});

builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Planner API", Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer sh4564jd6j...\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// builder.Services.Configure<ApiBehaviorOptions>(options =>
// {
//     options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(
//         new ErrorResponse) 
// });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializeAsync(userManager, roleManager);
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

async Task DbInitializeAsync(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager)
{
    const string adminEmail = "admin@admin.com";
    const string password = "string";
    
    if (await roleManager.FindByNameAsync("admin") is null)
        await roleManager.CreateAsync(new IdentityRole("admin"));
    
    if (await roleManager.FindByNameAsync("user") is null)
        await roleManager.CreateAsync(new IdentityRole("user"));

    if (await userManager.FindByNameAsync(adminEmail) is null)
    {
        var admin = new Account { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(admin, password);
        await userManager.AddToRoleAsync(admin, "admin");
    }
}