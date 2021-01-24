using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStore.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string ProductSKU { get; set; }

        [Required]
        public int ItemCount { get; set; }

        [Required]
        public DateTime DateOfTransaction { get; set; }

    }
}