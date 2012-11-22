using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model.BingService
{
    [DataContract]
    public class ResourceSet
    {
        [DataMember(Name = "estimatedTotal")]
        public long EstimatedTotal { get; set; }

        [DataMember(Name = "resources")]
        public Resource[] Resources { get; set; }
    }
}
