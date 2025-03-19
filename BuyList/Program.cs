using BusinessLogic.Exstensions;
using BusinessLogic.Services.BotServices;
using BuyList.Extensions;
using DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
var token = builder.Configuration["TelegramBot:Token"];

builder.Services.AddDbContext(connStr);
builder.Services.AddIdentity();
builder.Services.AddCustomServices();
builder.Services.AddRepositories();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<BotBackgroundService>();

var app = builder.Build();


app.DataBaseMigrate();
app.AddUploadingsFolder(Directory.GetCurrentDirectory());
app.SeedData(builder.Configuration).Wait();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
