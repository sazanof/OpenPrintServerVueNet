using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Hubs;
using OpenPrintServerVueNet.Server.Middlewares;
using OpenPrintServerVueNet.Server.Models;
using OpenPrintServerVueNet.Server.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opts.JsonSerializerOptions.PropertyNamingPolicy = null; // https://stackoverflow.com/questions/38202039/json-properties-now-lower-case-on-swap-from-asp-net-core-1-0-0-rc2-final-to-1-0
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWindowsService();
builder.Services.AddHostedService<JobBackgroundService>();
builder.Services.AddHostedService<SyncPrintersBackgroundService>();
builder.Services.AddHostedService<ApiSendBackgroundService>();
// �������� ������ ����������� �� ����� ������������
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
builder.Services.AddTransient<ApplicationContext>();

builder.Services.AddSignalR();

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/";
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<InstallMiddleware>();

app.MapControllers();

app.MapHub<NotificationsHub>("notifications");

app.MapFallbackToFile("/index.html");

app.Run();
