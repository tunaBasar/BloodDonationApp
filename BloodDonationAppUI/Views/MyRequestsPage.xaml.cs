using BloodDonationApp.Helpers;
using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace BloodDonationAppUI.Views;

public partial class MyRequestsPage : ContentPage
{
    private readonly IApiService apiService;
    public MyRequestsPage(IApiService apiService)
    {
        InitializeComponent();
        this.apiService = apiService;
        LoadMyRequests();
    }

    private async void LoadMyRequests()
    {
        var user = SessionManager.GetUser();
        var tc = new GetRequests
        {
            Tc = user.Tc
        };
        try
        {
            var requestsResponse = await apiService.GetRequestByTc(tc);
            if (requestsResponse.Success && requestsResponse.Data != null)
            {
                MyRequestsListView.ItemsSource = requestsResponse.Data;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler alınırken bir sorun oluştu: {ex.Message}", "Tamam");
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.CommandParameter is not int requestId)
        {
            await DisplayAlert("Hata", "Geçersiz Request ID", "Tamam");
            return;
        }

        string cityInput = await DisplayPromptAsync("Şehir Güncelle", "Yeni şehir adını giriniz (örn: Istanbul):");
        if (!Enum.TryParse(cityInput, out City parsedCity))
        {
            await DisplayAlert("Hata", "Geçersiz şehir adı", "Tamam");
            return;
        }

        string urgencyInput = await DisplayPromptAsync("Aciliyet Düzeyi", "Düzeyi giriniz (Low, Medium, High):");
        if (!Enum.TryParse(urgencyInput, out UrgencyLevel parsedUrgency))
        {
            await DisplayAlert("Hata", "Geçersiz aciliyet düzeyi", "Tamam");
            return;
        }

        var updateDto = new UpdateRequestDto
        {
            RequestId = requestId,
            City = parsedCity,
            UrgencyLevel = parsedUrgency
        };

        var response = await apiService.UpdateRequest(updateDto);

        if (response.Success)
        {
            await DisplayAlert("Başarılı", "Talep güncellendi.", "Tamam");
            LoadMyRequests();
        }
        else
        {
            await DisplayAlert("Hata", $"Güncelleme başarısız: {response.Message}", "Tamam");
        }
    }



    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Uyarı", "Bu talebi silmek istediğinizden emin misiniz?", "Evet", "Hayır");
        if (result)
        {
            var button = sender as Button;
            if (button?.CommandParameter is not int requestId)
            {
                await DisplayAlert("Hata", "Geçersiz Request ID", "Tamam");
                return;
            }
            var response = await apiService.DeleteRequest(requestId);
            if (response.Success)
            {
                await DisplayAlert("Başarılı", "Talep Silindi.", "Tamam");
                LoadMyRequests();
            }
            else
            {
                await DisplayAlert("Hata", $"Silme başarısız: {response.Message}", "Tamam");
            }

        }
    }
}