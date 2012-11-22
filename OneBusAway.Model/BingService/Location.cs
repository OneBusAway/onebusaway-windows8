using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model.BingService
{
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class Location : Resource
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "point")]
        public Point Point { get; set; }

        [DataMember(Name = "entityType")]
        public string EntityType { get; set; }

        [DataMember(Name = "address")]
        public Address Address { get; set; }
    }
}
