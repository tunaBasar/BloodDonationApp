using System;
using BloodDonationApp.Helpers;
using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace BloodDonationAppUI.Views
{
    public partial class CreateRequestPage : ContentPage
    {
        private readonly IApiService apiService;

        public CreateRequestPage(IApiService apiService)
        {
            InitializeComponent();
            this.apiService = apiService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var user = SessionManager.GetUser();
            if (user != null)
            {
                PhoneNumberLabel.Text = user.PhoneNumber;
                TcLabel.Text = user.Tc;
                BloodTypeLabel.Text = user.BloodType.ToString();
            }
        }
        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            try
            {
                if (CityPicker.SelectedItem == null || UrgencyPicker.SelectedItem == null)
                {
                    await DisplayAlert("Uyarı", "Lütfen tüm alanları doldurunuz.", "Tamam");
                    return;
                }

                var user = SessionManager.GetUser();
                if (user == null)
                {
                    await DisplayAlert("Hata", "Kullanıcı bilgileri bulunamadı.", "Tamam");
                    return;
                }

                var request = new CreateRequestModel()
                {
                    Tc = user.Tc,
                    BloodType = user.BloodType,
                    PhoneNumber=user.PhoneNumber,
                    City = (City)Enum.Parse(typeof(City), CityPicker.SelectedItem.ToString()),
                    UrgencyLevel = (UrgencyLevel)Enum.Parse(typeof(UrgencyLevel), UrgencyPicker.SelectedItem.ToString())
                };

                var response = await apiService.CreateRequest(request);

                if (response.Success)
                {
                    await DisplayAlert("Başarılı", "Talebiniz başarıyla oluşturuldu.", "Tamam");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Hata", response.Message ?? "Talep oluşturulamadı.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Talep oluşturulurken bir şeyler yanlış gitti: {ex.Message}", "Tamam");
            }
        }
    }
}
