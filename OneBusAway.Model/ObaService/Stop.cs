using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OneBusAway.Model
{
    /// <summary>
    /// Represents a bus stop.
    /// </summary>
    public class Stop : BindableBase
    {
        private string id;
        private double latitude;
        private double longitude;
        private string direction;
        private string name;
        private string code;
        private Route[] routes;
        private bool isClosestStop;        

        protected Stop()
        {
        }
                
        public Stop(XElement stopElement)
        {
            ParseStopElement(stopElement);
        }

        protected void ParseStopElement(XElement stopElement)
        {
            // Xml for a stop:
            //<stop>
            //    <id>3_3465</id>
            //    <lat>47.610756</lat>
            //    <lon>-122.337204</lon>
            //    <direction>NW</direction>
            //    <name>4th / Pike</name>
            //    <code>3465</code>
            //    <locationType>0</locationType>
            //    <routeIds>
            //      <string>40_590</string>
            //      <string>40_592</string>
            //      <string>40_594</string>
            //      <string>40_595</string>
            //    </routeIds>
            //  </stop>
            this.StopId = stopElement.GetFirstElementValue<string>("id");
            this.Latitude = stopElement.GetFirstElementValue<double>("lat");
            this.Longitude = stopElement.GetFirstElementValue<double>("lon");
            this.Direction = stopElement.GetFirstElementValue<string>("direction");
            this.Name = stopElement.GetFirstElementValue<string>("name");
            this.Code = stopElement.GetFirstElementValue<string>("code");

            HashSet<string> routeIds = new HashSet<string>(from routeString in stopElement.Element("routeIds").Descendants("string")
                                                           select routeString.Value);

            // The routes are stored at the begining of the document:
            this.Routes = (from routeElement in stopElement.Document.Descendants("route")
                           where routeIds.Contains(routeElement.GetFirstElementValue<string>("id"))
                           select new Route(routeElement)).ToArray();
        }

        public string StopId
        {
            get
            {
                return this.id;
            }
            set
            {
                SetProperty(ref this.id, value);
            }
        }

        public bool IsClosestStop
        {
            get
            {
                return this.isClosestStop;
            }
            set
            {
                SetProperty(ref this.isClosestStop, value);
            }
        }

        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                SetProperty(ref this.latitude, value);
            }
        }

        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                SetProperty(ref this.longitude, value);
            }
        }

        public string Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                SetProperty(ref this.direction, value);
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                SetProperty(ref this.name, value);
            }
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                SetProperty(ref this.code, value);
            }
        }

        public Route[] Routes
        {
            get
            {
                return this.routes;
            }
            set
            {
                SetProperty(ref this.routes, value);
            }
        }
    }
}