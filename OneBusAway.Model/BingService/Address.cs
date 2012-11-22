using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model.BingService
{
    [DataContract]
    public class Address
    {
        [DataMember(Name = "addressLine")]
        public string AddressLine { get; set; }

        [DataMember(Name = "adminDistrict")]
        public string AdminDistrict { get; set; }

        [DataMember(Name = "adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [DataMember(Name = "countryRegion")]
        public string CountryRegion { get; set; }

        [DataMember(Name = "formattedAddress")]
        public string FormattedAddress { get; set; }

        [DataMember(Name = "locality")]
        public string Locality { get; set; }

        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }

        [DataMember(Name = "neighborhood")]
        public string Neighborhood { get; set; }

        [DataMember(Name = "landmark")]
        public string Landmark { get; set; }
    }
}
