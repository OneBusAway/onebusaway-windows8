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
        private int code;
        private Route[] routes;

        public Stop()
        {
        }
                
        public Stop(XElement stopElement)
        {
            // Xml for a stop:
            //<stop>
            //<id>1_10914</id>
            //<lat>47.656426</lat>
            //<lon>-122.312164</lon>
            //<direction>S</direction>
            //<name>15TH AVE NE &amp; NE CAMPUS PKWY</name>
            //<code>10914</code>
            //<locationType>0</locationType>
            //<routes>
            //    <route>
            //    <id>1_43</id>
            //    <shortName>43</shortName>
            //    <description>cbd/u-district</description>
            //    <type>3</type>
            //    <url>http://metro.kingcounty.gov/tops/bus/schedules/s043_0_.html</url>
            //    <agency>
            //        <id>1</id>
            //        <name>Metro Transit</name>
            //        <url>http://metro.kingcounty.gov</url>
            //        <timezone>America/Los_Angeles</timezone>
            //        <lang>en</lang>
            //        <phone>206-553-3000</phone>
            //        <disclaimer>Transit scheduling, geographic, and real-time data provided by permission of King County.  Some real-time info provided by UW Intelligent Transportation Systems.</disclaimer>
            //        <privateService>false</privateService>
            //    </agency>
            //    </route>
            //</routes>
            //</stop>
            this.Id = stopElement.GetFirstElementValue<string>("id");
            this.Latitude = stopElement.GetFirstElementValue<double>("lat");
            this.Longitude = stopElement.GetFirstElementValue<double>("lon");
            this.Direction = stopElement.GetFirstElementValue<string>("direction");
            this.Name = stopElement.GetFirstElementValue<string>("name");
            this.Code = stopElement.GetFirstElementValue<int>("code");

            this.Routes = (from routeElement in stopElement.Descendants("route")
                           select new Route(routeElement)).ToArray();
        }

        [XmlElement(ElementName = "id")]
        public string Id
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

        [XmlElement(ElementName = "lat")]
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

        [XmlElement(ElementName = "lon")]
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

        [XmlElement(ElementName = "direction")]
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

        [XmlElement(ElementName = "name")]
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

        [XmlElement(ElementName = "code")]
        public int Code
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

        [XmlArray(ElementName="routes")]        
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
