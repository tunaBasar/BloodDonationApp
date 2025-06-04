using Microsoft.Extensions.Logging;
using BloodDonationAppUI.Services;
using BloodDonationAppUI.Services.Interfaces;
using BloodDonationAppUI.Views;

namespace BloodDonationAppUI;

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
            });


        builder.Services.AddHttpClient();
        

        builder.Services.AddScoped<IApiService, ApiService>();
        
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<UserRegisterPage>();
        builder.Services.AddTransient<UserLoginPage>();
        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<UserHomePage>();
        builder.Services.AddTransient<CreateRequestPage>();
        builder.Services.AddTransient<MyRequestsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}