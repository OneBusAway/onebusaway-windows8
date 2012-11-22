using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model.BingService
{
    [DataContract]
    public class Point
    {
        [DataMember(Name = "coordinates")]
        public double[] Coordinates { get; set; }
    }
}
