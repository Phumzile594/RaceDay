
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models
{
    public class Profile
    {
        public int ProfileID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Emergency contact name is required.")]
        public string EmergencyContactName { get; set; }
            = string.Empty;

        [Required(ErrorMessage = "Emergency contact phone is required.")]
        [Phone]
        public string EmergencyContactPhone { get; set; }
            = string.Empty;
    }
}
