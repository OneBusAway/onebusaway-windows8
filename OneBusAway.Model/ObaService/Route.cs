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
    /// This class represents a bus route.
    /// </summary>
    public class Route : BindableBase
    {
        private string id;
        private string shortName;
        private string description;
        private string scheduleUri;
        private Agency agency;

        public Route()
        {
        }

        public Route(XElement routeElement)
        {
            // Xml for route:
            // <route>
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
            // </route>

            this.Id = routeElement.GetFirstElementValue<string>("id");
            this.ShortName = routeElement.GetFirstElementValue<string>("shortName");
            this.Description = routeElement.GetFirstElementValue<string>("description");
            this.ScheduleUri = routeElement.GetFirstElementValue<string>("url");

            if (string.IsNullOrEmpty(this.Description))
            {
                this.Description = routeElement.GetFirstElementValue<string>("longName");
            }

            // Throw if there is no agency.  There should always be an agency:
            var agencyElement = routeElement.Descendants("agency").FirstOrDefault();
            if (agencyElement != null)
            {
                this.Agency = new Agency(agencyElement);
            }
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

        [XmlElement(ElementName = "shortname")]
        public string ShortName
        {
            get
            {
                return this.shortName;
            }
            set
            {
                SetProperty(ref this.shortName, value);
            }
        }

        [XmlElement(ElementName = "description")]
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                SetProperty(ref this.description, value);
            }
        }

        [XmlElement(ElementName = "url")]
        public string ScheduleUri
        {
            get
            {
                return this.scheduleUri;
            }
            set
            {
                SetProperty(ref this.scheduleUri, value);
            }
        }

        [XmlElement(ElementName = "agency")]
        public Agency Agency
        {
            get
            {
                return this.agency;
            }
            set
            {
                SetProperty(ref this.agency, value);
            }
        }

        /// <summary>
        /// Override to string so that we format this object into a nice XML string.
        /// </summary>
        public XElement ToXElement()
        {
            // <route>
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
            // </route>
            var root = new XElement("route",
                new XElement("id", this.Id),
                new XElement("shortName", this.shortName),
                new XElement("description", this.description),
                new XElement("url", this.scheduleUri));

            if (this.agency != null)
            {
                root.Add(this.agency.ToXElement());
            }

            return root;
        }
    }
}
