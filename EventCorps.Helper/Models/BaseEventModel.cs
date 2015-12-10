using System;
using System.Collections.Specialized;
using EventCorps.Helper.Enums;

namespace EventCorps.Helper.Models
{
    public class EventModel: CreateEventModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool Expired { get; set; }
        public bool IsBookedOut { get; set; }
        public int NumberFree { get; set; }
        public int NumberWaitingList { get; set; }
    }
}