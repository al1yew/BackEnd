using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewModels
{
    public class PaginationList<T> : List<T>
    {
        public PaginationList(IQueryable<T> brands, int page, int pagecount, int brandcount)
        {
            HasNext = page < pagecount;
            HasPrev = page > 1;
            PageCount = pagecount;
            Page = page;
            BrandCount = brandcount;
            this.AddRange(brands);
        }

        public int Page { get; }
        public int PageCount { get; }
        public bool HasPrev { get; }
        public bool HasNext { get; }
        public int BrandCount { get; }

        public static PaginationList<T> Create(IQueryable<T> brands, int page, int brandCount)
        {
            int pageCount = (int)Math.Ceiling((decimal)brands.Count() / brandCount);

            page = page < 1 || page > pageCount ? 1 : page;

            brands = brands.Skip((page - 1) * brandCount).Take(brandCount);

            return new PaginationList<T>(brands, page, pageCount, brandCount);
        }
    }
}
