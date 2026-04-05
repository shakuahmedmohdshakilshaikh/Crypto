using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }


        public static ApiResponse<T> SuccessResponse(T data, string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Error = null
            };
        }


        public static ApiResponse<T> ErrorResponse(string errorMessage)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Failed",
                Data = default,
                Error = errorMessage
            };
        }

    }
}
