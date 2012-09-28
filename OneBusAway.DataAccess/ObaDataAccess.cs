using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

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
        public async Task GetStopsForLocationAsync(double latitude, double longitude)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.stops_for_location, HttpMethod.GET);            
            helper.AddToQueryString("lat", latitude.ToString(CultureInfo.CurrentCulture));
            helper.AddToQueryString("lon", longitude.ToString(CultureInfo.CurrentCulture));
            
//            <response>
//  <version>1</version>
//  <code>200</code>
//  <currentTime>1348786676170</currentTime>
//  <text>OK</text>
//  <data class="org.onebusaway.transit_data.model.StopsBean">
//    <stops>
//      <stop>
//        <id>1_10914</id>
//        <lat>47.656426</lat>
//        <lon>-122.312164</lon>
//        <direction>S</direction>
//        <name>15TH AVE NE &amp; NE CAMPUS PKWY</name>
//        <code>10914</code>
//        <locationType>0</locationType>
//        <routes>
//          <route>
//            <id>1_43</id>
//            <shortName>43</shortName>
//            <description>cbd/u-district</description>
//            <type>3</type>
//            <url>http://metro.kingcounty.gov/tops/bus/schedules/s043_0_.html</url>
//            <agency>
//              <id>1</id>
//              <name>Metro Transit</name>
//              <url>http://metro.kingcounty.gov</url>
//              <timezone>America/Los_Angeles</timezone>
//              <lang>en</lang>
//              <phone>206-553-3000</phone>
//              <disclaimer>Transit scheduling, geographic, and real-time data provided by permission of King County.  Some real-time info provided by UW Intelligent Transportation Systems.</disclaimer>
//              <privateService>false</privateService>
//            </agency>
//          </route>
//          ...
//        </routes>
//      </stop>
//      ..
//    </stops>
//    <limitExceeded>false</limitExceeded>
//  </data>
//</response>

            string response = await helper.SendAndRecieveAsync();

        }
    }
}
