﻿using TatBlog.Core.Contracts;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class PagingParams : IPagingParams
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string SortColumn { get; set; } = "Id";
        public string SortOrder { get; set; } = "ASC";
    }
}
