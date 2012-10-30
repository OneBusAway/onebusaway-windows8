using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// Represents a bus agency.
    /// </summary>
    public class Agency : BindableBase
    {
        private string websiteUri;
        private string name;
        private string phoneNumber;
        private string disclaimer;
        private string uri;

        /// <summary>
        /// Creates the agency.
        /// </summary>
        public Agency()
        {
        }

        public Agency(XElement agencyElement)
        {
            // xml for agency:
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
            this.Id = agencyElement.GetFirstElementValue<string>("id");
            this.Name = agencyElement.GetFirstElementValue<string>("name");
            this.WebsiteUri = agencyElement.GetFirstElementValue<string>("url");
            this.PhoneNumber = agencyElement.GetFirstElementValue<string>("phone");
            this.Disclaimer = agencyElement.GetFirstElementValue<string>("disclaimer");
        }

        public string Id
        {
            get
            {
                return this.websiteUri;
            }
            set
            {
                SetProperty(ref this.websiteUri, value);
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

        public string WebsiteUri
        {
            get
            {
                return this.websiteUri;
            }
            set
            {
                SetProperty(ref this.websiteUri, value);
            }
        }

        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }
            set
            {
                SetProperty(ref this.phoneNumber, value);
            }
        }

        public string Disclaimer
        {
            get
            {
                return this.disclaimer;
            }
            set
            {
                SetProperty(ref this.disclaimer, value);
            }
        }
    }
}
