using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Server.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        // https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
        // Enable split queries globally
        // See if this boosts speed
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))); 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
       .AddDefaultIdentity<ApplicationUser>(options =>
       {
           options.SignIn.RequireConfirmedAccount = true;
           options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
           options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
           options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
       })
       .AddRoles<IdentityRole>() // TRY ADD ROLES
       .AddRoleManager<RoleManager<IdentityRole>>() // TRY ADD ROLES 
       .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentityServer()
//    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

// TRY ADD ROLES
builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
    {
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityServerJwt();

// Add Model Services for Controllers
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseInstance_ExercisePropertyRepository, ExerciseInstance_ExercisePropertyRepository>();
builder.Services.AddScoped<IExerciseInstanceRepository, ExerciseInstanceRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IWorkoutProgramRepository, WorkoutProgramRepository>();
builder.Services.AddScoped<IExercisePropertyRepository, ExercisePropertyRepository>();
builder.Services.AddScoped<IBiometricInformationRepository, BiometricInformationRepository>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

//builder.Services.AddScoped<UserManager<ApplicationUser>>(); // doesn't help

//builder.Services.AddControllersWithViews();

// https://stackoverflow.com/a/62985944
// Allow 2 objects to have navigation properties to each other
builder.Services.AddControllersWithViews()
       .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
