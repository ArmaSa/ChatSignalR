using SignalRProject.Hub;
using Extention;
using Common;
using Data.Context;
using Data;

var builder = WebApplication.CreateBuilder(args);
var ConfigurationBinder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR(e => { 
    e.MaximumReceiveMessageSize = 102400000;
    e.EnableDetailedErrors = true;
});

builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>();

builder.Services.Configure<ApplicationSettings>(options => builder.Configuration.Bind(options));

builder.Services.AddDbContext(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHub<ChatHub>("/chatHub");


await app.RunAsync("http://172.16.7.62:5003");
