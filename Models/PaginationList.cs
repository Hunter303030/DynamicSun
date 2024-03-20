using Microsoft.EntityFrameworkCore;

namespace DynamicSun.Models
{
    public class PaginationList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; set; }

        public PaginationList(List<T> item, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(item);
        }

        public bool PreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }
        public bool NextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public static async Task<PaginationList<T>> CreateAsyns(IEnumerable<T> sourse, int pageIndex, int pageSize)
        {
            var count = sourse.Count();
            var item = sourse.Skip((pageIndex-1)* pageSize).Take(pageSize).ToList();
            return new PaginationList<T>(item, count, pageIndex, pageSize);
        }
    }
}
