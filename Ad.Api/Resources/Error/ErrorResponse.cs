using System.Collections.Generic;

namespace Ad.API.Resources
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
            ResponseCode = "99";
        }

        public string ResponseCode { get; set; }
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}