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
            else
            {
                await DisplayAlert("Hata", $"Talepler yüklenemedi: {requestsResponse.Message}", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler alınırken bir sorun oluştu: {ex.Message}", "Tamam");
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Info", "Talep düzenleniyor", "Tamam");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Uyarı", "Bu talebi silmek istediğinizden emin misiniz?", "Evet", "Hayır");
        if (result)
        {
            await DisplayAlert("Info", "Talep siliniyor", "Tamam");
        }
    }
}