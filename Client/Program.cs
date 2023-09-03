using CogniFitRepo.Client;
using CogniFitRepo.Client.HttpServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("CogniFitRepo.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CogniFitRepo.ServerAPI"));

//Register HttpServices
builder.Services.AddScoped<IWorkoutProgramHttpService, WorkoutProgramHttpService>();
builder.Services.AddScoped<IWorkoutHttpService, WorkoutHttpService>();
builder.Services.AddScoped<IExerciseInstanceHttpService, ExerciseInstanceHttpService>();
builder.Services.AddScoped<IExerciseHttpService, ExerciseHttpService>();
builder.Services.AddScoped<IApplicationUserHttpService, ApplicationUserHttpService>();
builder.Services.AddScoped<IBiometricInformationHttpService, BiometricInformationHttpService>();
builder.Services.AddScoped<IExercisePropertyHttpService, ExercisePropertyHttpService>();

//Radzen Service Registration
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
