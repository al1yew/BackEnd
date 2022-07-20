using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public Category Parent { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Image { get; set; }
        public List<Category> Children { get; set; }

    }
}
