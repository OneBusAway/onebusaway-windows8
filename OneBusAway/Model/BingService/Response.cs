/* Copyright 2014 Michael Braude and individual contributors.
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
    public class Response
    {
        [DataMember(Name = "copyright")]
        public string Copyright { get; set; }

        [DataMember(Name = "brandLogoUri")]
        public string BrandLogoUri { get; set; }

        [DataMember(Name = "statusCode")]
        public int StatusCode { get; set; }

        [DataMember(Name = "statusDescription")]
        public string StatusDescription { get; set; }

        [DataMember(Name = "authenticationResultCode")]
        public string AuthenticationResultCode { get; set; }

        [DataMember(Name = "errorDetails")]
        public string[] errorDetails { get; set; }

        [DataMember(Name = "traceId")]
        public string TraceId { get; set; }

        [DataMember(Name = "resourceSets")]
        public ResourceSet[] ResourceSets { get; set; }
    }
}
