﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PaginatedResult()
        {
        }

        public PaginatedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }

        public static PaginatedResult<T> Create(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            return new PaginatedResult<T>(items, totalCount, page, pageSize);
        }
    }
}