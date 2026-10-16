
namespace RaceDay.API.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public int UserID { get; set; }

        public string EventName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public string Location { get; set; } = string.Empty;

        public decimal DistanceKm { get; set; }

        public string EventType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
