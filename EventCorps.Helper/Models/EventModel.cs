using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using EventCorps.Helper.Enums;

namespace EventCorps.Helper.Models
{
    public class EventModel: CreateEventModel
    {
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public string Url { get; set; }
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public bool Expired { get; set; }
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public bool IsBookedOut { get; set; }
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public int NumberFree { get; set; }
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public int NumberWaitingList { get; set; }
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public bool UserHasSubscribed { get; set; }
    }
}