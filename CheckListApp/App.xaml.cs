using CheckListApp.Data;
using CheckListApp.Services;
using CheckListApp.View;

namespace CheckListApp;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Services = serviceProvider;

        MainPage = new AppShell();

        InitializeAppAsync();
        Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
    }

    private async void InitializeAppAsync()
    {
        try
        {
            var database = Services.GetRequiredService<TaskDatabase>();
            await database.InitializeDatabaseAsync();
        }
        catch (InvalidOperationException ex)
        {
            // Handle database initialization failure
            Console.WriteLine($"Failed to initialize database: {ex.Message}");
            // You might want to show an error message to the user or take other appropriate action
        }
    }
}