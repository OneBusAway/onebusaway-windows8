using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
