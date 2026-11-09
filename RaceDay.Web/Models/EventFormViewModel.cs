
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class EventFormViewModel
    {
        public int EventID { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        [StringLength(150, MinimumLength = 3)]
        [Display(Name = "Event Name")]
        public string EventName { get; set; } = "";

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Event date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; } =
            new TimeSpan(7, 0, 0);

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(150)]
        public string Location { get; set; } = "";

        [Range(0.01, 1000, ErrorMessage =
            "Distance must be greater than zero.")]
        [Display(Name = "Distance (km)")]
        public decimal DistanceKm { get; set; }

        [Required]
        [Display(Name = "Event Type")]
        public string EventType { get; set; } = "Run";

        [Required]
        public string Status { get; set; } = "Upcoming";
    }
}
