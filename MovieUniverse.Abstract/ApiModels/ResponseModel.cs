using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MovieUniverse.Abstract.ApiModels
{
    [DataContract]
    
    public class ResponseModel
    {
        [DataMember]
        public const bool HasError = false;
        [DataMember]
        public HttpStatusCode StatusCode { get; set; }
        [DataMember]
        public object Response { get; set; }
        [DataMember]
        public string Message { get; set; }

    }
}
