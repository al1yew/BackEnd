using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Data.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
