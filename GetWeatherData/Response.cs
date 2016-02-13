using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace RESTServicesJSONParser
{
    [DataContract]
    public class Response
    {
        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }
        [DataMember(Name = "postal_code")]
        public string postal_code { get; set; }
        [DataMember(Name = "tempMin")]
        public decimal tempMin { get; set; }
        [DataMember(Name = "tempAvg")]
        public decimal tempAvg { get; set; }
        [DataMember(Name = "tempMax")]
        public decimal tempMax { get; set; }
        [DataMember(Name = "cldCvrAvg")]
        public decimal cldCvrAvg { get; set; }
        [DataMember(Name = "snowfall")]
        public decimal snowfall { get; set; }
        [DataMember(Name = "precip")]
        public decimal precip { get; set; }    

    }
}
