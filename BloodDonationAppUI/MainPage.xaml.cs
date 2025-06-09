using BloodDonationAppUI.Services.Interfaces;
using BloodDonationAppUI.Views;
using Microsoft.Maui.Controls;

namespace BloodDonationAppUI;

public partial class MainPage : ContentPage
{
    private readonly IApiService apiService;

    public MainPage(IApiService apiService)
    {
        InitializeComponent();
        this.apiService = apiService;
    }

    private async void OnDonateBloodClicked(object sender, EventArgs e)
    {
        var registerPage = ServiceHelper.GetService<UserRegisterPage>();
        await DisplayAlert("Bağış yapmak için önce kayıt olmalısınız", "Kayıt sayfasına yönlendiriliyorsunuz", "Tamam");
        await Navigation.PushAsync(registerPage);
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            var registerPage = ServiceHelper.GetService<UserRegisterPage>();
            await Navigation.PushAsync(registerPage);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Sayfa yüklenirken hata oluştu: {ex.Message}", "Tamam");
        }
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var loginPage = ServiceHelper.GetService<UserLoginPage>();
            await Navigation.PushAsync(loginPage);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Sayfa yüklenirken hata oluştu: {ex.Message}", "Tamam");
        }
    }

    private async void OnEmergencyClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Acil Kan İhtiyacı",
            "Bu acil bir durumdur. Hemen Kızılay'ın resmi kan bağışı sayfasına gitmek istiyor musunuz?",
            "Evet", "Hayır");

        if (answer)
        {
            try
            {
                await Launcher.OpenAsync("https://www.kizilay.org.tr/");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Tarayıcı açılamadı", "Tamam");
            }
        }
    }

}