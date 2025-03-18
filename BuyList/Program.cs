using BusinessLogic.Exstensions;
using DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
var token = builder.Configuration["TelegramBot:Token"];

builder.Services.AddDbContext(connStr);
builder.Services.AddIdentity();
builder.Services.AddCustomServices();
builder.Services.AddRepositories();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHostedService<BotBackgroundService>();

var app = builder.Build();



app.DataBaseMigrate();
app.AddUploadingsFolder(Directory.GetCurrentDirectory());

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
