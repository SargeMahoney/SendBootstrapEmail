using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using SendBootstrapEmail.Configuration;
using SendBootstrapEmail.Data;
using SendBootstrapEmail.EmailServices;
using SendBootstrapEmail.TemplateServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();



builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddSingleton<IEmailConfiguration>(sp => sp.GetRequiredService<IOptions<EmailConfiguration>>().Value);
builder.Services.AddTransient<IEmailSender,EmailSender>();
builder.Services.AddTransient<ITemplateService, TemplateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
