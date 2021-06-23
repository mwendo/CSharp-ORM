using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}
        [Required]
        public string WedderOne {get;set;}
        [Required]
        public string WedderTwo {get;set;}
        [Required]
        [DataType(DataType.Date)]
        public DateTime WeddingDate {get;set;}
        [Required]
        public string WeddingAddress {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<RSVP> Guests {get;set;}

        [Required]
        public int UserId {get;set;}
        public User Planner {get;set;}
    }
}