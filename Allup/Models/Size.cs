using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allup.Models
{
    public class Size
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        [Required]
        public string SizeName { get; set; }
        public IEnumerable<ProductToSize> ProductToSizes { get; set; }

    }
}
