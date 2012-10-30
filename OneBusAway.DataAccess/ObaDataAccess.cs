using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml.Linq;
using OneBusAway.Model;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// Class talks to the OneBusAway server and grabs data from it.
    /// </summary>
    public class ObaDataAccess
    {
        /// <summary>
        /// Creates the data access.
        /// </summary>
        public ObaDataAccess()
        {
            this.Factory = new ObaServiceHelperFactory(Constants.SERVER_URL);
        }

        /// <summary>
        /// The service factory allows us to create web requests.
        /// </summary>
        public ObaServiceHelperFactory Factory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the stops near a location.
        /// </summary>
        public async Task<Stop[]> GetStopsForLocationAsync(double latitude, double longitude)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.stops_for_location, HttpMethod.GET);            
            helper.AddToQueryString("lat", latitude.ToString(CultureInfo.CurrentCulture));
            helper.AddToQueryString("lon", longitude.ToString(CultureInfo.CurrentCulture));

            XDocument doc = await helper.SendAndRecieveAsync();
            return (from stopElement in doc.Descendants("stop")
                    select new Stop(stopElement)).ToArray();
        }
    }
}
