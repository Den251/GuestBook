using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Models
{
    public class Record
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }
        

    }
}
