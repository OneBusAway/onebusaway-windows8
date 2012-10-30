using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBusAway.DataAccess;
using OneBusAway.DataAccess.Fakes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.DataAccess.Test
{
    [TestClass]
    public class ObaDataAccessTest
    {
        [TestMethod]
        public async Task TestGetStops()
        {
            ObaDataAccess access = new ObaDataAccess();

            var factoryStub = new StubObaServiceHelperFactory(Constants.SERVER_URL);
            var helperStub = new StubIObaServiceHelper();

            factoryStub.CreateHelperObaMethodHttpMethod = (obaMethod, httpMethod) => helperStub;

            int queryStrCount = 0;

            string xml = strings.getStopsXml;
            helperStub.SendAndRecieveAsyncString = payload => Task.FromResult<XDocument>(XDocument.Parse(xml));
            helperStub.AddToQueryStringStringString = (key, val) => queryStrCount++;

            access.Factory = factoryStub;

            var stops = await access.GetStopsForLocationAsync(47.653435, -122.305641);

            Assert.AreEqual(2, queryStrCount);
            Assert.AreEqual(1, stops.Length);
        }        

        [TestMethod]
        public async Task TestGetStopsIntegrationTest()
        {
            ObaDataAccess access = new ObaDataAccess();
            var stops = await access.GetStopsForLocationAsync(47.653435, -122.305641);
        }
    }
}
