using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkPlayground.Domain.Models
{
    public class PagingResourceParameters
    {
        const int maximumPageSize = 10;
        private int pageSize = 5;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > maximumPageSize) ? maximumPageSize : value;
        }
    }
}
