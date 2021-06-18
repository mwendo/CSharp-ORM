using System;
using System.ComponentModel.DataAnnotations;

namespace CRUDelicious.Models
{
    
    public class Dish
    {
        [Key]
        
        [Required]
        public int DishesId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Chef { get; set; }
        [Required]
        [Range(0,5)]
        public int Tastiness {get;set;}
        [Required]
        [Range(0, 5000)]
        public int Calories {get;set;}
        [Required]
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}