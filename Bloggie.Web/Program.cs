using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using static AnalyticsRepository;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Access configuration
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Databases
builder.Services.AddDbContext<BloggieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

// Configure Identity with Default Token Providers
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders(); // ?? Ensures token generation works!


builder.Services.Configure<IdentityOptions>(options =>
{
    // Default settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRepository, FileUploadRepository>();
builder.Services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>();
builder.Services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.HttpContext.Request.Cookies["jwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },

        OnChallenge = context =>
        {
            context.HandleResponse();

            var requestPath = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            var encodedReturnUrl = Uri.EscapeDataString(requestPath);

            context.HttpContext.Response.Redirect($"/Account/Login?returnUrl={encodedReturnUrl}");
            return Task.CompletedTask;
        },

        OnForbidden = context =>
        {
            context.Response.Redirect("/Account/AccessDenied");
            return Task.CompletedTask;
        }
    };



    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});




var app = builder.Build();
// Enable serving static files from wwwroot
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<VisitTrackerMiddleware>();

app.UseHttpsRedirection();


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
