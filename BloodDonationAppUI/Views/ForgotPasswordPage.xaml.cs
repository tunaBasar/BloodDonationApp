using System.Text.RegularExpressions;
using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services;
using BloodDonationAppUI.Services.Interfaces;

namespace BloodDonationAppUI.Views;

public partial class ForgotPasswordPage : ContentPage
{
    private readonly IApiService apiService;
    public ForgotPasswordPage(IApiService apiService)
    {
        InitializeComponent();
        this.apiService = apiService;
    }

    private async void OnUpdatePasswordClicked(object sender, EventArgs e)
    {
        try
        {

            if (!ValidateInputs())
                return;
            try
            {
                var user = new UserForgotPassword()
                {
                    Mail = EmailEntry.Text,
                    Tc = TcEntry.Text,
                    Password = NewPasswordEntry.Text
                };
                var changedUser = await apiService.ForgotPassword(user);
                ClearForm();
                await DisplayAlert("Başarılı", "Şifre değiştirme işlemi!", "Tamam");
                await Shell.Current.GoToAsync("..");

            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.Message.Contains("500"))
                {
                    await DisplayAlert("Hata", "Kullanıcı bulunamadı. Lütfen TC kimlik numaranızı ve Mailinizi kontrol edin.", "Tamam");
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
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Şifre güncellenirken bir hata oluştu. Lütfen tekrar deneyin.", "Tamam");
        }
        finally
        {

            var button = sender as Button;
            button.Text = "Şifremi Güncelle";
            button.IsEnabled = true;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private bool ValidateInputs()
    {

        if (string.IsNullOrWhiteSpace(TcEntry.Text))
        {
            ShowValidationError("TC Kimlik No boş bırakılamaz.");
            TcEntry.Focus();
            return false;
        }

        if (TcEntry.Text.Length != 11)
        {
            ShowValidationError("TC Kimlik No 11 haneli olmalıdır.");
            TcEntry.Focus();
            return false;
        }

        if (!IsValidTcKimlik(TcEntry.Text))
        {
            ShowValidationError("Geçerli bir TC Kimlik No giriniz.");
            TcEntry.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            ShowValidationError("E-mail adresi boş bırakılamaz.");
            EmailEntry.Focus();
            return false;
        }

        if (!IsValidEmail(EmailEntry.Text))
        {
            ShowValidationError("Geçerli bir e-mail adresi giriniz.");
            EmailEntry.Focus();
            return false;
        }

        // Password validation
        if (string.IsNullOrWhiteSpace(NewPasswordEntry.Text))
        {
            ShowValidationError("Yeni şifre boş bırakılamaz.");
            NewPasswordEntry.Focus();
            return false;
        }

        if (NewPasswordEntry.Text.Length < 6)
        {
            ShowValidationError("Şifre en az 6 karakter olmalıdır.");
            NewPasswordEntry.Focus();
            return false;
        }

        // Password confirmation validation
        if (string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
        {
            ShowValidationError("Şifre tekrarı boş bırakılamaz.");
            ConfirmPasswordEntry.Focus();
            return false;
        }

        if (NewPasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            ShowValidationError("Şifreler eşleşmiyor. Lütfen kontrol ediniz.");
            ConfirmPasswordEntry.Focus();
            return false;
        }

        return true;
    }

    private bool IsValidTcKimlik(string tcKimlik)
    {
        // Basic TC Kimlik validation
        if (tcKimlik.Length != 11)
            return false;

        if (!tcKimlik.All(char.IsDigit))
            return false;

        if (tcKimlik[0] == '0')
            return false;



        bool check1 = true;



        return check1;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    private async void ShowValidationError(string message)
    {
        await DisplayAlert("Uyarı", message, "Tamam");
    }

    private void ClearForm()
    {
        TcEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        NewPasswordEntry.Text = string.Empty;
        ConfirmPasswordEntry.Text = string.Empty;
    }

    private void OnEmailEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (!string.IsNullOrEmpty(entry.Text))
        {
            entry.Text = entry.Text.ToLower();
        }
    }
}