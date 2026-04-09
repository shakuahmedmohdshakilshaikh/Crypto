using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; }  = new List<T>();

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public bool HasNext { get; set; }

        public int Nextpage { get; set; }

        public int PreviousPage { get; set; }

        public bool HasPrevious { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        
    }
}
