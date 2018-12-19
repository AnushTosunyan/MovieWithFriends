using System.Runtime.Serialization;

namespace MovieUniverse.Abstract.ApiModels
{
    [DataContract]
    public class ResponseObject
    {
        [DataMember]
        public object Data { get; set; }
        [DataMember]
        public long Total { get; set; }
    }
}