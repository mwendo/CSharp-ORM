using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltExam.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string LoginEmail {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string LoginPassword {get;set;}
    }
}