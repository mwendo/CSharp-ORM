using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsAndCategories
{
    public class Test
    {
        [Key]
        public int TestId {get;set;}
        public string Name {get;set;}
    }
}