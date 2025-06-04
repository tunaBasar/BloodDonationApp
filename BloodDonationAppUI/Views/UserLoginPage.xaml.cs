using BloodDonationApp.Helpers;
using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services.Interfaces;

namespace BloodDonationAppUI.Views
{
    public partial class UserLoginPage : ContentPage
    {
        private readonly IApiService apiService;

        public UserLoginPage(IApiService apiService)
        {
            InitializeComponent();
            this.apiService = apiService;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (await ValidateForm())
            {
                await ProcessLogin();
            }
        }
        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            try
            {
                var forgotpasswordpage = ServiceHelper.GetService<ForgotPasswordPage>();
                await Navigation.PushAsync(forgotpasswordpage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Sayfa yüklenirken hata oluştu: {ex.Message}", "Tamam");
            }
        }

        private async Task ProcessLogin()
        {
            try
            {
                var user = new UserLoginModel()
                {
                    Tc = TcEntry.Text,
                    Password = PasswordEntry.Text
                };
                var loginUser = await apiService.LoginUser(user);
                if (loginUser.Success)
                {
                    SessionManager.SetUser(loginUser.Data);
                    var homePage = ServiceHelper.GetService<UserHomePage>();
                    await Navigation.PushAsync(homePage);
                }
                else
                {
                    await DisplayAlert("Başarısız", "Şifre yanlış!", "Tamam");
                }

            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.Message.Contains("500"))
                {
                    await DisplayAlert("Hata", "Kullanıcı bulunamadı. Lütfen TC kimlik numaranızı ve şifrenizi kontrol edin.", "Tamam");
                }
                else
                {
                    await DisplayAlert("Hata", "Sunucu ile bağlantı kurulamadı. Lütfen internet bağlantınızı kontrol edin.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Beklenmeyen bir hata oluştu: {ex.Message}", "Tamam");
            }
        }

        private async Task<bool> ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(TcEntry.Text) || TcEntry.Text.Length != 11)
            {
                await DisplayAlert("Hata", "TC Kimlik numarası 11 haneli olmalıdır.", "Tamam");
                return false;
            }
            return true;
        }
    }
}