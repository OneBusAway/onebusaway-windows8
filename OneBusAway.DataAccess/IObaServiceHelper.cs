using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// This interface defines an object that talks to a web service.
    /// </summary>
    public interface IObaServiceHelper
    {
        /// <summary>
        /// Adds a name / value pair to the query string.
        /// </summary>
        void AddToQueryString(string name, string value);

        /// <summary>
        /// Sends a payload to the service asynchronously and reads the response.
        /// </summary>
        Task<string> SendAndRecieveAsync(string payload = null);
    }
}
