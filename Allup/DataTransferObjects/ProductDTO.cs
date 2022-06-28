//using Allup.Models;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Allup.DataTransferObjects
//{
//    public class ProductDTO
//    {
//        public List<IFormFile> Files { get; set; }

//        [Required(ErrorMessage = "The Product Name field is required.")]
//        [StringLength(maximumLength: 255)]
//        public string ProductName { get; set; }

//        [Required]
//        [Column(TypeName = "money")]
//        public double Price { get; set; }

//        [Column(TypeName = "money")]
//        public double DiscountPrice { get; set; }

//        [Required]
//        [Column(TypeName = "money")]
//        public double ExTax { get; set; }

//        [StringLength(maximumLength: 1000)]
//        public string Description { get; set; }

//        [Range(0, int.MaxValue)]
//        public int Count { get; set; }

//        //public Brand Brand { get; set; }
//        public Nullable<int> BrandId { get; set; }
//        //public Category Category { get; set; }
//        public Nullable<int> CategoryId { get; set; }

//        public bool IsBestSeller { get; set; }
//        public bool IsFeature { get; set; }

//    }
//}
