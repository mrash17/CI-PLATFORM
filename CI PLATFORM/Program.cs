
using ciplatform.entities.Data;
using ciplatform.repository.Interface;
using ciplatform.repository.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CidbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IUserInterface, UserRepository>();
builder.Services.AddScoped<ILandingPageInterface, LandingPageRepository>();
builder.Services.AddScoped<IVolunteerMissionInterface, VolunteerMissionRepository>();
builder.Services.AddScoped<IStoryListingInterface, StoryListingRepository>();
builder.Services.AddScoped<ITimeSheetInterface, TimeSheetRepository>();
builder.Services.AddScoped<IAdminInterface, AdminRepository>();

builder.Services.AddSession();
builder.Services.AddCloudscribePagination();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    option.LoginPath = "/User/Index";
    option.AccessDeniedPath = "/User/Index";
});
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(10);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
//For Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomAPIController", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//For Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomAPIController");
    // Add additional configurations if needed
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LandingPage}/{action=landingpage}/{id?}");

app.Run();
