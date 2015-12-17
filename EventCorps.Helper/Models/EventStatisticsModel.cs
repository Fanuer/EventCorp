namespace EventCorps.Helper.Models
{
    public class EventStatisticsModel
    {
        public string Url { get; set; }
        public long EventAll { get; set; }
        public long EventsOpen { get; set; }
        public long EventsClosed { get; set; }
        public EventModel MostSuccessful { get; set; }
        public double AverageFillLevel { get; set; }
        public string PlaceMostEvents { get; set; }
    }
}