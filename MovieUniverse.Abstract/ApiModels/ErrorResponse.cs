using System.Net;
using System.Net.Http;

namespace MovieUniverse.Abstract.ApiModels
{
    public class ErrorResponse:HttpResponseMessage
    {
        public const bool HasError = true;
        
        public int ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

    }
}