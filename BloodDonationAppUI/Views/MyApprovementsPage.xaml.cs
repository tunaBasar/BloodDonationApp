using System.Diagnostics;
using BloodDonationApp.Helpers;
using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services.Interfaces;
using BloodDonationAppUserService;

namespace BloodDonationAppUI.Views
{

    public partial class MyApprovementsPage : ContentPage
    {
        private readonly IApiService apiService;

        public MyApprovementsPage(IApiService apiService)
        {
            InitializeComponent();
            this.apiService = apiService;
            LoadMyApprovement();
        }

        private async void LoadMyApprovement()
        {
            var user = SessionManager.GetUser();
            try
            {
                var approvementResponse = await apiService.GetMyApprovementsByTc(user.Tc);
                if (approvementResponse.Success && approvementResponse.Data != null)
                {
                    ApprovalRequestsListView.ItemsSource = approvementResponse.Data;
                }
                else
                {
                    await DisplayAlert("Hata", $"İstekler yüklenemedi: {approvementResponse.Message}", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Veriler alınırken bir sorun oluştu: {ex.Message}", "Tamam");
            }
        }

        private async void OnApproveClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var selectedApprovement = button?.BindingContext as ApproveDto;

                if (selectedApprovement == null)
                {
                    await DisplayAlert("Hata", "Seçilen talep bulunamadı.", "Tamam");
                    return;
                }

                var approvement = new CreateDonationDto
                {
                    ApprovementId = selectedApprovement.Id,
                    RequestId=selectedApprovement.RequestId
                };
                var result = await apiService.CreateDonationAsync(approvement);

                if (result.Success)
                {
                    await DisplayAlert("Başarılı", "Bağış talebiniz onaylandı!", "Tamam");
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
        private async void OnRejectClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var selectedApprovement = button?.BindingContext as ApproveDto;

                if (selectedApprovement == null)
                {
                    await DisplayAlert("Hata", "Seçilen talep bulunamadı.", "Tamam");
                    return;
                }
                var result = await apiService.RejectApprove(selectedApprovement.Id);

                if (result.Success)
                {
                    await DisplayAlert("Başarılı", "Bağış talebiniz onaylandı!", "Tamam");
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
    }

}