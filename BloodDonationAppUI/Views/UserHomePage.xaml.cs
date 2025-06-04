using System;
using BloodDonationApp.Helpers;
using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace BloodDonationAppUI.Views
{
    public partial class UserHomePage : ContentPage
    {
        private readonly IApiService apiService;
        public UserHomePage(IApiService apiService)
        {
            InitializeComponent();
            this.apiService = apiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var user = SessionManager.GetUser();
                if (user != null)
                {

                    FullNameLabel.Text = $"{user.FirstName} {user.LastName}";
                    TcLabel.Text = user.Tc;
                    BloodTypeLabel.Text = user.BloodType.ToString();
                    CityLabel.Text = user.City.ToString();

                    var requestsResponse = await apiService.GetRequestByBloodType((int)user.BloodType);

                    if (requestsResponse.Success && requestsResponse.Data != null)
                    {
                        RequestListView.ItemsSource = requestsResponse.Data;
                    }
                    else
                    {
                        await DisplayAlert("Hata", $"Talepler yüklenemedi: {requestsResponse.Message}", "Tamam");
                    }
                }


            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Veriler alınırken bir sorun oluştu: {ex.Message}", "Tamam");
            }
        }

        private async void OnDonateButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var selectedRequest = button?.BindingContext as RequestResponseModel;
                string json = System.Text.Json.JsonSerializer.Serialize(selectedRequest);
                await DisplayAlert("DEBUG JSON", json, "Tamam");

                if (selectedRequest == null)
                {
                    await DisplayAlert("Hata", "Seçilen talep bulunamadı.", "Tamam");
                    return;
                }
                var user = SessionManager.GetUser();
                var request = new DonationRequest
                {
                    ApproveDto = new ApproveDto
                    {
                        UserDonorTc = user.Tc,
                        Request = new RequestModel
                        {
                            Id = selectedRequest.Id,
                            UserTc = selectedRequest.Tc,
                            BloodType = (int)selectedRequest.BloodType,
                            UrgencyLevel = (int)selectedRequest.UrgencyLevel,
                            City = (int)selectedRequest.City
                        }
                    }
                };

                var result = await apiService.SendDonationRequestAsync<object>(request);

                if (result.Success)
                {
                    await DisplayAlert("Başarılı", "Bağış talebiniz gönderildi!", "Tamam");

                    // İsteğe bağlı: Başarılı bağış sonrası listeyi yenile
                    // await RefreshRequestList();
                }
                else
                {
                    await DisplayAlert("Hata", result.Message, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Beklenmeyen hata: {ex.Message}", "Tamam");
            }
        }

        private async void OnTalepteBulunClicked(object sender, EventArgs e)
        {
            try
            {
                var createrequestpage = ServiceHelper.GetService<CreateRequestPage>();
                await Navigation.PushAsync(createrequestpage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Sayfa yüklenirken hata oluştu: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}", "Tamam");
            }
        }

        private async void OnTaleplerimiGorClicked(object sender, EventArgs e)
        {
            try
            {
                var myRequestsPage = ServiceHelper.GetService<MyRequestsPage>();
                await Navigation.PushAsync(myRequestsPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Sayfa yüklenirken hata oluştu: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}", "Tamam");
            }


        }

        private async void OnCikisYapClicked(object sender, EventArgs e)
        {
            SessionManager.ClearSession();
            var loginPage = ServiceHelper.GetService<UserLoginPage>();
            await Navigation.PushAsync(loginPage);
        }
    }
}
