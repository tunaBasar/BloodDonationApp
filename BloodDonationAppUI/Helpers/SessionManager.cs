
using BloodDonationAppUI.Models;

namespace BloodDonationApp.Helpers;

public static class SessionManager
{
    private static UserResponseModel _currentUser;

    public static void SetUser(UserResponseModel user)
    {
        _currentUser = user;
    }

    public static UserResponseModel GetUser()
    {
        return _currentUser;
    }

    public static bool IsUserLoggedIn => _currentUser != null;

    public static void ClearSession()
    {
        _currentUser = null;
    }
}