﻿namespace ShopOwnerCore.Application_Core.Helper.Pager
{
    public class PageList<T>:List<T>
    {
        public MetaData MetaData { get; set; }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count/(double)pageSize)


            };
            AddRange(items);
        }

        public static PageList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PageList<T>(items, count, pageNumber, pageSize);

        }
    }
}
