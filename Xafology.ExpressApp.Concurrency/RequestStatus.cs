using System;
using System.Linq;
using System.Collections.Generic;

namespace Xafology.ExpressApp.Concurrency
{
    public enum RequestStatus
    {
        Waiting,
        Processing,
        Complete,
        Error,
        Cancelled
    }
}
