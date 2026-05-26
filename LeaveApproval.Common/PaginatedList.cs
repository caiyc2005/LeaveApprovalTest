using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Common
{
    /// <summary>
    /// 分页组件
    /// </summary>
    public class PaginatedList<T> : List<T>
    {
        //页索引
        public int PageIndex { get; private set; }
        //总页数
        public int TotalPages { get;private set;  }

        public PaginatedList(List<T> items,int count,int  pageIndex,int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }
        //上一页是否可用
        public bool HasPreviousPage => PageIndex > 1;
        //下一页是否可用
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source,int pageIndex,int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex-1)*pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }
}
