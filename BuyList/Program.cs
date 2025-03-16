using BusinessLogic.Exstensions;
using DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext(connStr);
builder.Services.AddCustomServices();
builder.Services.AddRepositories();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

//app.DataBaseMigrate();
//app.AddUploadingsFolder(Directory.GetCurrentDirectory());

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
