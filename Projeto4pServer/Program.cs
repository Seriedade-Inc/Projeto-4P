using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Scalar.AspNetCore;
using Projeto4pServer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Projeto4pSharedLibrary.Services;
using Projeto4pServer.Repository;
// Add the correct namespace for UserRepository below if it's in a different namespace
// Example:


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserRepository , UserRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IAgendaRepository, AgendaRepository>();
builder.Services.AddScoped<IBlasphemyRepository, BlasphemyRepository>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<ICharacterSkillsRepository, CharacterSkillsRepository>();
builder.Services.AddScoped<AgendaService>();
builder.Services.AddScoped<BlasphemyService>();
builder.Services.AddScoped<CharacterService>();
builder.Services.AddScoped<CharacterSkillsService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddHttpContextAccessor();
BuilderService buildingService = new();

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
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
buildingService.CheckAuth();

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
app.UseStaticFiles();  
app.UseCors("BlazorPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); 

app.Run();