namespace BloodDonationAppUI.Models
{
    public class UserCreateModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Tc { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public City City { get; set; }
        public BloodType BloodType { get; set; }
    }

    public enum BloodType
    {
        APositive,
        ANegative,
        BPositive,
        BNegative,
        ABPositive,
        ABNegative,
        OPositive,
        ONegative
    }
    
    public enum City
    {
        Istanbul,
        Ankara,
        Izmır,
        Antalya,
        Konya
    }
}