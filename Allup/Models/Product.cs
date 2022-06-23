using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allup.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        public string ProductName { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public double Price { get; set; }
        [Column(TypeName = "money")]
        public double DiscountPrice { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public double ExTax { get; set; }
        [Required]
        [StringLength(maximumLength: 4)]
        public string Seria { get; set; }
        [Range(0, 9999)]
        [Column(TypeName = "int")]
        [Required]
        public int Code { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        [StringLength(maximumLength: 255)]
        public string MainImage { get; set; }
        [StringLength(maximumLength: 255)]
        public string HoverImage { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsFeature { get; set; }
        public IEnumerable<ProductToColor> ProductToColors { get; set; }
        public IEnumerable<ProductToSize> ProductToSizes { get; set; }
        public IEnumerable<ProductToTag> ProductToTags { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public Category Category { get; set; }
        public Nullable<int> CategoryId { get; set; }
    }
}
