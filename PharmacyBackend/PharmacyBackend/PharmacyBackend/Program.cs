using Microsoft.EntityFrameworkCore;
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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
#endregion

//TINE SUBUKAN MO NGA BRANCH MO

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("RacquelPharmacy");

app.UseAuthorization();

app.MapControllers();

app.Run();
