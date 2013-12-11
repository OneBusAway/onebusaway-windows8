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

        public string FullName
        {
            get
            {
                return Name;
            }
        }
    }
}
