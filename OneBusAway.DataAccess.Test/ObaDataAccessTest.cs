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
        private ObaDataAccess dataAccess;
        private StubObaServiceHelperFactory factoryStub;
        private StubIObaServiceHelper helperStub;

        [TestInitialize]
        public void TestInitialize()
        {
            this.dataAccess = new ObaDataAccess();

            this.factoryStub = new StubObaServiceHelperFactory(Constants.SERVER_URL);
            this.helperStub = new StubIObaServiceHelper();

            factoryStub.CreateHelperObaMethodHttpMethod = (obaMethod, httpMethod) => this.helperStub;
            this.dataAccess.Factory = this.factoryStub;
        }


        [TestMethod]
        public async Task TestGetStops()
        {
            int queryStrCount = 0;
            string xml = strings.getStopsXml;

            this.helperStub.SendAndRecieveAsyncString = payload => Task.FromResult<XDocument>(XDocument.Parse(xml));
            this.helperStub.AddToQueryStringStringString = (key, val) => queryStrCount++;

            var stops = await this.dataAccess.GetStopsForLocationAsync(47.653435, -122.305641);

            Assert.AreEqual(2, queryStrCount);
            Assert.AreEqual(1, stops.Length);
        }                

        [TestMethod]
        public async Task TestGetRoutesForStopAsync()
        {
            string xml = strings.getRoutesXml;
            this.helperStub.SendAndRecieveAsyncString = payload => Task.FromResult<XDocument>(XDocument.Parse(xml));            

            var routes = await this.dataAccess.GetRoutesForStopAsync("1_75403");

            Assert.AreNotEqual(0, routes.Length);
        }

        [TestMethod]
        public async Task TestGetStopsForRouteAsync()
        {
            ObaDataAccess access = new ObaDataAccess();

            string xml = strings.getStopsForRouteXml;
            this.helperStub.SendAndRecieveAsyncString = payload => Task.FromResult<XDocument>(XDocument.Parse(xml));

            var stops = await access.GetStopsForRouteAsync("1_44");
            Assert.AreNotEqual(0, stops.Length);
        }

        [TestMethod]
        public async Task TestGetShapeForRouteAsync()
        {
            ObaDataAccess access = new ObaDataAccess();            

            string xml = strings.getStopsForRouteXml;
            this.helperStub.SendAndRecieveAsyncString = payload => Task.FromResult<XDocument>(XDocument.Parse(xml));

            var shape = await access.GetShapeForRouteAsync("1_40046045");
            Assert.AreNotEqual(0, shape.Points.Length);
        }

        [TestMethod]
        public async Task TestGetTrackingDataForStopAsync()
        {
            ObaDataAccess access = new ObaDataAccess();

            string xml = strings.getTrackingDataForStopXml;
            this.helperStub.SendAndRecieveAsyncString = payload => Task.FromResult<XDocument>(XDocument.Parse(xml));

            var trackingData = await access.GetTrackingDataForStopAsync("1_75403");
            Assert.AreNotEqual(0, trackingData.Length);
        }
    }
}
