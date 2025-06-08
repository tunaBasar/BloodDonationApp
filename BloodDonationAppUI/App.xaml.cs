using BloodDonationAppUI.Views;
namespace BloodDonationAppUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Routing.RegisterRoute("mainpage", typeof(MainPage));
        Routing.RegisterRoute("register", typeof(UserRegisterPage));
        Routing.RegisterRoute("login", typeof(UserLoginPage));
        Routing.RegisterRoute("forgot", typeof(ForgotPasswordPage));
        Routing.RegisterRoute("Home", typeof(UserHomePage));
        Routing.RegisterRoute("CreateRequest", typeof(CreateRequestPage));
        Routing.RegisterRoute("MyRequests", typeof(MyRequestsPage));
        Routing.RegisterRoute("MyApprovements", typeof(MyApprovementsPage));

    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}