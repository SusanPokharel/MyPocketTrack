using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using MudBlazor;
using MyPocketTrack.Components.Services;
namespace MyPocketTrack;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        
        var transactionService = new TransactionService();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        builder.Services.AddSingleton(transactionService);
        builder.Services.AddSingleton<TransactionService>();
#endif
        builder.Services.AddMudServices();
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = true;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
        });
        
        builder.Services.AddSingleton<UserServices>();
        builder.Services.AddSingleton<TransactionService>();
        
        return builder.Build();
    }
}