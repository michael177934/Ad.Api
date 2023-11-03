using Ad.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Test
{
    public class SuccessResponse<T>
    {
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        public T ResponseData { get; set; }
        public object ExtraData { get; set; }
    }

    public class ErrorResponse
    {
        public string ResponseCode { get; set; }
        public string Message { get; set; }
    }

    public class APIResponse
    {
        public static SuccessResponse<T> Success<T>(string message, T data = default, object extraData = null)
            where T : class
        {
            return new SuccessResponse<T>
            {
                ResponseCode = ResponseStatusCode.Success,
                Message = message,
                ResponseData = data,
                ExtraData = extraData
            };
        }

        public static ErrorResponse Error(string message)
        {
            return new ErrorResponse
            {
                ResponseCode = ResponseStatusCode.Failed,
                Message = message
            };
        }
    }
}
