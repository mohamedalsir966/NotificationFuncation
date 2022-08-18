using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationFunction.Entitys
{
    public class PaginatedList<T>
    {
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }
        public List<T> Items { get; set; } = new List<T>();

        public PaginatedList(IEnumerable<T> items, int take, int skip, int pageSize, int totalItems = 0)
        {
            Skip = skip;
            TotalPages = (int)Math.Ceiling(take / (double)pageSize);
            TotalItems = totalItems;
            this.Items?.AddRange(items);
        }

        //public static async Task<PaginatedList<T>> CreateAsync(IEnumerable<T> source, int skip, int take)
        //{
        //    var count = source.Count();
        //    var items = source.Skip(skip)
        //        .Take(take);
        //    return new PaginatedList<T>(items, count, skip, take, count);
        //}
    }
}
