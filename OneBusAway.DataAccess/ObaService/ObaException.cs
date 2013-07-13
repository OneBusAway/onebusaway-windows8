<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// Specific exception for OBA.
    /// </summary>
    public class ObaException : Exception
    {
        /// <summary>
        /// Creates the Oba exception.
        /// </summary>
        public ObaException(int errorCode, string errorText)
            : base(string.Format(CultureInfo.CurrentCulture, "OBA Error {0}: {1}", errorCode, errorText))
        {
            this.ErrorCode = errorCode;
            this.ErrorText = errorText;
        }

        public int ErrorCode
        {
            get;
            private set;
        }

        public string ErrorText
        {
            get;
            private set;
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.DataAccess.ObaService
{
    /// <summary>
    /// Specific exception for OBA.
    /// </summary>
    public class ObaException : Exception
    {
        /// <summary>
        /// Creates the Oba exception.
        /// </summary>
        public ObaException(int errorCode, string errorText)
            : base(string.Format(CultureInfo.CurrentCulture, "OBA Error {0}: {1}", errorCode, errorText))
        {
            this.ErrorCode = errorCode;
            this.ErrorText = errorText;
        }

        public int ErrorCode
        {
            get;
            private set;
        }

        public string ErrorText
        {
            get;
            private set;
        }
    }
}
>>>>>>> 5fa5eaa... Re-arranging the view models by moving page controls & user control view models into sudifferent namepsaces / folders.
