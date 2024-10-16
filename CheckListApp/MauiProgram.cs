using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CheckListApp.Services;
using CheckListApp.ViewModels;
using CheckListApp.View;
using CheckListApp.Data;
using CommunityToolkit.Maui;

namespace CheckListApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit();

        // Configure logging (if needed)
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        // Register database
        builder.Services.AddSingleton<TaskDatabase>();

        // Register services
        builder.Services.AddSingleton<UserTaskService>();
        builder.Services.AddSingleton<AuthenticationService>();
        builder.Services.AddSingleton<UserService>();

        // Register view models
        builder.Services.AddTransient<UserTaskViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();

        // Register pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<UserTaskPage>();
        builder.Services.AddTransient<ItemDetailPage>();
      


        return builder.Build();
    }
}