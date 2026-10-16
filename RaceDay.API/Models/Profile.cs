
namespace RaceDay.API.Models
{
    public class Profile
    {
        public int ProfileID { get; set; }

        public int UserID { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? EmergencyContactName { get; set; }

        public string? EmergencyContactPhone { get; set; }
    }
}
