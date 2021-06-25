using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BeltExam.Models
{
    public class FutureDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime) value < DateTime.Now)
                return new ValidationResult("Date must be in the future");
            return ValidationResult.Success;
        }
    }

    public class Entertainment
    {
        [Key]
        public int EntertainmentId {get;set;}
        [Required]
        public string Title {get;set;}
        [Required]
        [DataType(DataType.Time)]
        public DateTime Time {get;set;}
        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime EntertainmentDate {get;set;} 
        [Required]
        public int Duration {get;set;}
        [Required]
        public string DurationAmount {get;set;}
        [Required]
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Participant> Participants {get;set;}
        public int UserId {get;set;}
        public User Coordinator {get;set;}
    }
}