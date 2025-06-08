using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services.Interfaces;
using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace BloodDonationAppUI.Views;

public partial class UserRegisterPage : ContentPage
{
    private readonly IApiService apiService;
    
    public UserRegisterPage(IApiService apiService)
    {
        InitializeComponent();
        this.apiService = apiService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (await ValidateForm())
        {
            await ProcessRegistration();
        }
    }

    private async Task<bool> ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) || string.IsNullOrWhiteSpace(LastNameEntry.Text))
        {
            await DisplayAlert("Hata", "Ad ve soyad alanları boş bırakılamaz.", "Tamam");
            return false;
        }

        if (string.IsNullOrWhiteSpace(TCNumberEntry.Text) || TCNumberEntry.Text.Length != 11)
        {
            await DisplayAlert("Hata", "TC Kimlik numarası 11 haneli olmalıdır.", "Tamam");
            return false;
        }

        if (!IsValidTCNumber(TCNumberEntry.Text))
        {
            await DisplayAlert("Hata", "Geçerli bir TC Kimlik numarası giriniz.", "Tamam");
            return false;
        }

        if (string.IsNullOrWhiteSpace(PhoneEntry?.Text))
        {
            await DisplayAlert("Hata", "Telefon numarası boş bırakılamaz.", "Tamam");
            return false;
        }

        if (!IsValidPhoneNumber(PhoneEntry.Text))
        {
            await DisplayAlert("Hata", "Geçerli bir telefon numarası giriniz.", "Tamam");
            return false;
        }

        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlert("Hata", "E-posta adresi boş bırakılamaz.", "Tamam");
            return false;
        }

        if (!IsValidEmail(EmailEntry.Text))
        {
            await DisplayAlert("Hata", "Geçerli bir e-posta adresi giriniz.", "Tamam");
            return false;
        }

        if (BloodGroupPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Hata", "Kan grubunuzu seçiniz.", "Tamam");
            return false;
        }

        var age = DateTime.Now.Year - BirthDatePicker.Date.Year;
        if (BirthDatePicker.Date.Date > DateTime.Now.AddYears(-age)) age--;

        if (age < 18 || age > 65)
        {
            await DisplayAlert("Hata", "Kan bağışı için yaşınız 18-65 arasında olmalıdır.", "Tamam");
            return false;
        }

        if (!TermsCheckBox.IsChecked)
        {
            await DisplayAlert("Hata", "Kullanım şartlarını kabul etmelisiniz.", "Tamam");
            return false;
        }

        if (!ContactCheckBox.IsChecked)
        {
            await DisplayAlert("Hata", "Acil durumlarda iletişim onayı gereklidir.", "Tamam");
            return false;
        }

        return true;
    }

    private async Task ProcessRegistration()
    {
        try
        {
            var user = new UserCreateModel()
            {
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text,
                Tc = TCNumberEntry.Text,
                BirthDate = BirthDatePicker.Date,
                PhoneNumber=PhoneEntry.Text,
                Mail = EmailEntry.Text,
                Password = PasswordEntry.Text,
                City = (City)Enum.Parse(typeof(City), CityGroupPicker.SelectedItem?.ToString() ?? "Istanbul"),
                BloodType = (BloodType)Enum.Parse(typeof(BloodType), BloodGroupPicker.SelectedItem?.ToString() ?? "APositive"),
            };
            
            var registeredUser = await apiService.RegisterUser(user);
            
            await DisplayAlert("Başarılı", "Kayıt işlemi başarıyla tamamlandı!", "Tamam");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Kayıt işlemi sırasında bir hata oluştu: {ex.Message}", "Tamam");
        }
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Giriş Yap", "Giriş sayfasına yönlendiriliyorsunuz...", "Tamam");
        await Navigation.PopAsync();
    }

    #region Validation Methods

    private bool IsValidTCNumber(string tcNumber)
    {
        if (string.IsNullOrEmpty(tcNumber) || tcNumber.Length != 11)
            return false;

        if (!tcNumber.All(char.IsDigit))
            return false;

        if (tcNumber[0] == '0')
            return false;
            
        int[] digits = tcNumber.Select(c => int.Parse(c.ToString())).ToArray();

        bool check1 = true;

        return check1;
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        var regex = new Regex(@"^(\+90|0)?(5\d{2})\s?(\d{3})\s?(\d{2})\s?(\d{2})$");
        return regex.IsMatch(phone.Replace(" ", "").Replace("-", ""));
    }

    #endregion
}