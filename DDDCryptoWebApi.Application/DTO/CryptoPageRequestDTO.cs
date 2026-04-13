using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class CryptoPageRequestDTO
    {
        public string? Currency { get; set; }
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public string? SearchText { get; set; } = "";

        public string? SortBy { get; set; } = "";

        public string? SortOrder { get; set; } = "asc";
    }

}
