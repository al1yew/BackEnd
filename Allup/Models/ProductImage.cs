using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allup.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:1000)]
        public string Image { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
