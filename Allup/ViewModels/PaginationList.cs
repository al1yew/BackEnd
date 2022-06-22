using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewModels
{
    public class PaginationList<T> : List<T>
    {
        public PaginationList()
        {

        }
        public int Page { get; }
        public int PageCount { get; }
        public bool HasPrev { get; }
        public bool HasNext { get; }
    }
}
