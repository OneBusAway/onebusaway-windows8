using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        [XmlElement(ElementName="id")]
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
    }
}
