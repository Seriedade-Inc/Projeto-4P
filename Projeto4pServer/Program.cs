using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Scalar.AspNetCore;
using Projeto4pServer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //options.LoginPath = "/api/user/login"; //! Caminho do login, apenas quando chegar la pfv
        //options.LogoutPath = "/api/user/logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(10);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<EmailService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseCors("BlazorPolicy");
app.UseAuthorization();
app.MapControllers();   

app.Run();