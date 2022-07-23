using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.DTOs.CategoryDTOs
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentCategory { get; set; }
    }
}
