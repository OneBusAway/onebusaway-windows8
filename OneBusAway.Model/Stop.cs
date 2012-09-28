using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
