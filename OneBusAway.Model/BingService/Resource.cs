using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model.BingService
{
    [KnownType(typeof(Location))]
    [DataContract]
    public class Resource
    {
        [DataMember(Name = "bbox")]
        public double[] BoundingBox { get; set; }

        [DataMember(Name = "__type")]
        public string Type { get; set; }

        [DataMember(Name = "confidence")]
        public string Confidence { get; set; }
    }
}
