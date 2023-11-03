using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Core.Response
{
    public class OperationResponse<T>
    {
        public OperationResponse(string message, T data = default, bool success = true)
        {
            Message = message;
            Data = data;
            Success = success;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
