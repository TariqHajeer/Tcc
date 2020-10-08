using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO
{
    public class PagingDTO
    {
        int rowCount = 10;
        public int RowCount { get => rowCount; set => rowCount = Math.Min(30,value); }
        public int Page { get; set; } = 1;
    }
}
