using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.Contracts.ServiceContracts.DeleteInterface;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.Helper;
using PharmacyBackend.Repository.DELETE;
using PharmacyBackend.Repository.GET;
using PharmacyBackend.Repository.POST;
using PharmacyBackend.Repository.UpdateRepository;
using PharmacyBackend.Service.DeleteService;
using PharmacyBackend.Service.GetService;
using PharmacyBackend.Service.PostService;
using PharmacyBackend.Service.UpdateService;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
          {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddDbContext<PharmacyContext>(x => x.UseNpgsql(
    builder.Configuration.GetConnectionString("RacquelDB")));

builder.Services.AddCors(x => x.AddPolicy(name: "RacquelPharmacy", x =>
{
    x.AllowAnyOrigin()
     .AllowAnyHeader()
     .AllowAnyMethod();
}));

#region AddScopes
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<IGetRepositoryContracts, GetRepository>();
builder.Services.AddScoped<IPostRepositoryContracts, PostRepository>();
builder.Services.AddScoped<IDeleteRepositoryContracts, DeleteRepository>();
builder.Services.AddScoped<IUpdateRepositoryContracts, UpdateRepository>();
builder.Services.AddScoped<IGetServiceContracts, GetService>();
builder.Services.AddScoped<IPostServiceContracts, PostService>();
builder.Services.AddScoped<IDeleteServiceContracts, DeleteService>();
builder.Services.AddScoped<IUpdateServiceContracts, UpdateService>();
builder.Services.AddScoped<AccountHelper>();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("RacquelPharmacy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();