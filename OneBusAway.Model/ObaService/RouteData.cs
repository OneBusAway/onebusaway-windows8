using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// Contains data about a route - the location of it's stops and it's polyline.
    /// </summary>
    public class RouteData : BindableBase
    {
        private Stop[] stops;
        private Shape[] shapes;

        /// <summary>
        /// Creates a route data with stops from both routes.
        /// </summary>
        public RouteData(XElement dataElement, XElement groupStopElement)
        {
            this.Initialize(dataElement, groupStopElement);
        }

        /// <summary>
        /// Creates a RouteData object out of a dataElement. See
        /// 
        /// http://api.onebusaway.org/api/where/stops-for-route/1_44.xml?key=TEST&version=2
        /// 
        /// For an example.
        /// </summary>
        public RouteData(XElement dataElement, string stopId)
        {
            XElement foundStopIdElement = (from stopIdsElement in dataElement.Descendants("stopIds").Elements("string")
                                           where string.Equals(stopId, stopIdsElement.Value, StringComparison.OrdinalIgnoreCase)
                                           select stopIdsElement).FirstOrDefault();

            if (foundStopIdElement == null)
            {
                throw new ArgumentException(string.Format("Unknown trip stop '{0}'", stopId));
            }

            this.Initialize(dataElement, foundStopIdElement.Parent.Parent);
        }

        /// <summary>
        /// Creates a route data out of a specific collection of group stop elements.
        /// </summary>
        private void Initialize(XElement dataElement, XElement stopGroupElement)
        {
            //<stopGroupings>
            //    <stopGrouping>
            //        <type>direction</type>
            //        <ordered>true</ordered>
            //        <stopGroups>
            //            <stopGroup>
            //                <id>0</id>
            //                <name>
            //                    <type>destination</type>
            //                    <names>
            //                        <string>BALLARD, WALLINGFORD</string>
            //                    </names>
            //                </name>
            //                <stopIds>
            //                    <string>1_25751</string>
            //                    <string>1_29410</string>
            //                    ...
            //                </stopIds>
            //                <polylines>
            //                    <encodedPolyline>
            //                        <points>gsxaHzqniV]Ds@Ba@?[?g@?Q?uAFE?QLyGCaA?kCBQ@MFW`@Wf@_B`Ew@pBUh@qAbDa@~@}@~BcAfCuCfHIVcAfCcBzDY]YUUGkKQ_C?o@?wBCiHGgHE{CEiBBa@@?rBApB?nB?tB?pBAtB?pBApB?dA?f@AdB?xC?fA?h@AtB?f@?jA?dCAxD?`@An@?l@?\?`AAv@AjD@zE?rEAtEAhE?nC?lFAlF?lFAjF?|F?N?N?lE?V?Z?xD?~@?lD?xB?L?xG?pG?V?hDqAzCi@xAKXGRADAJ?@?DApE?V@bABR@VDr@?f@@bB?V?b@?|BAf@C`@E^AXAd@AfB?|A@`H?vF?xFA`HW|AWt@[x@GLyA|Cg@x@GFa@d@a@d@mC`DgBrBi@d@CBKHw@h@yAr@uE`Cu@b@KHgAv@a@\YT[T]Vg@^OZKVIPG^EZE|@@fA?nC@vE?lB@zA?NAfN?lL?xH@dF?N?TA|N?J?`F?^?dGAdH?rG?nA?bE?lF?d@?p@?bI?rF@`O?L@lO?hI@bFBvO|E?</points>
            //                        <length>184</length>
            //                    </encodedPolyline>
            //                    ...
            //                </polylines>
            //        </stopGroup>
            //        ...            

            // Now find the stops that are part of this trip headsign:
            var stopIdsQuery = from stringElement in stopGroupElement.Descendants("stopIds").First().Descendants("string")
                                select stringElement.Value;

            // Now we need to find all of the stops that are part of this trip. Under the Data element will be a routes element:
            //<stop>
            //<id>1_10914</id>
            //<lat>47.656422</lat>
            //<lon>-122.312164</lon>
            //<direction>S</direction>
            //<name>15TH AVE NE & NE CAMPUS PKWY</name>
            //<code>10914</code>
            //<locationType>0</locationType>
            //<routeIds>
            //  <string>1_43</string>
            //  <string>1_44</string>
            //</routeIds>
            //</stop>
            XElement stopsElement = dataElement.Descendants("stops").First();
            this.Stops = (from stopIdElement in stopIdsQuery
                          join stopElement in stopsElement.Descendants("stop") on stopIdElement equals stopElement.GetFirstElementValue<string>("id")
                          select new Stop(stopElement)).ToArray();

            // Lastly, store the polylines:
            this.Shapes = (from encodedPolylineElement in stopGroupElement.Descendants("encodedPolyline")
                           select new Shape(encodedPolylineElement)).ToArray();
        }

        public Stop[] Stops
        {
            get
            {
                return this.stops;
            }
            set
            {
                SetProperty(ref this.stops, value);
            }
        }

        public Shape[] Shapes
        {
            get
            {
                return this.shapes;
            }
            set
            {
                SetProperty(ref this.shapes, value);
            }
        }
    }
}
