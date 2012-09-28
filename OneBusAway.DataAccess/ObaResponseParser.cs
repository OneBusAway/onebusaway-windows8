using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// Helper class helps us parse an Oba response and read the xml that came back from the server.
    /// </summary>
    public class ObaResponseParser : IDisposable
    {
        /// <summary>
        /// The xml to parse.
        /// </summary>
        private string xml;

        /// <summary>
        /// This is the reader.
        /// </summary>
        private XmlReader reader;

        /// <summary>
        /// Parses the response.
        /// </summary>
        public ObaResponseParser(string xml)
        {
            this.reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings()
            {
                Async = true,
                CloseInput = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true,                
            });
        }

        /// <summary>
        /// Disposes of the parser.
        /// </summary>
        public void Dispose()
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
        }

        /// <summary>
        /// Recursively parses the xml document and returns an instance of type T from the document.
        /// </summary>
        public async Task<T> Parse<T>()
        {
            // Read the header and barf if we got an error:
            

            await Task.Delay(0);
            return default(T);
        }

        private T ConvertTo<T>()
        {
            return default(T);
        }
    }
}
