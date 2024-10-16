using CheckListApp;
using CheckListApp.View;
using Microsoft.Maui;
using Microsoft.Extensions.DependencyInjection;


namespace CheckListApp;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(UserTaskPage), typeof(UserTaskPage));

        // Navigate to MainPage using DI to ensure the MainPageViewModel is provided
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }

    // Example of navigating to MainPage with DI
    public async Task NavigateToMainPage()
    {
        // Access the service provider through the App's current instance
        var mainPage = (MainPage)App.Current.Handler.MauiContext.Services.GetService(typeof(MainPage));

        // Navigate to MainPage
        await Shell.Current.GoToAsync(nameof(MainPage));
    }

}
