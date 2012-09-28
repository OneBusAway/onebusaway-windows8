using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBusAway.DataAccess;
using OneBusAway.DataAccess.Fakes;
using System.Threading.Tasks;

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
            helperStub.SendAndRecieveAsyncString = payload => Task.FromResult<string>(String.Empty);
            helperStub.AddToQueryStringStringString = (key, val) => queryStrCount++;

            access.Factory = factoryStub;

            await access.GetStopsForLocationAsync(47.653435, -122.305641);

            Assert.AreEqual(2, queryStrCount);
        }

        [TestMethod]
        public async Task TestGetStopsIntegrationTest()
        {
            ObaDataAccess access = new ObaDataAccess();
            await access.GetStopsForLocationAsync(47.653435, -122.305641);
        }
    }
}
