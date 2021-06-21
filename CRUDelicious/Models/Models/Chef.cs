using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDelicious.Models
{
    public class Chef
    {
        [Key]
        [Required]
        public int ChefId {get;set;}
        [Required]
        public string FirstName {get;set;}
        [Required]
        public string LastName {get; set;}
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        //Navigation property for related Dish objects
        public List<Dish> CreatedDishes {get;set;}
    }
}