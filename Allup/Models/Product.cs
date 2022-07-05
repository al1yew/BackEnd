using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Allup.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Product Name field is required.")]
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

        [StringLength(maximumLength: 4)]
        public string Seria { get; set; }
        
        [Range(0, 9999)]
        public int Code { get; set; }

        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Count { get; set; }

        [StringLength(maximumLength: 255)]
        public string MainImage { get; set; }
        [StringLength(maximumLength: 255)]
        public string HoverImage { get; set; }

        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsFeature { get; set; }

        public IEnumerable<ProductToColor> ProductToColors { get; set; }
        public IEnumerable<ProductToSize> ProductToSizes { get; set; }
        public IEnumerable<ProductToTag> ProductToTags { get; set; }
        public List<ProductImage> ProductImages { get; set; }

        public Brand Brand { get; set; }
        public Nullable<int> BrandId { get; set; }

        public Category Category { get; set; }
        public Nullable<int> CategoryId { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }


        [NotMapped]
        public IFormFile MainFile { get; set; }
        [NotMapped]
        public IFormFile HoveredFile { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
