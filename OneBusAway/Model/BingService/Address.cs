/* Copyright 2013 Michael Braude and individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
