using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import;

namespace Xafology.ImportDemo.UnitTests
{
    using DevExpress.ExpressApp;
    using ExpressApp.Xpo.ValueMap;
    using SDP.ParserUtils;

    public class ForexRateInserter
    {
        public ForexRateInserter(ImportForexParam param, Stream stream, XpoFieldMapper xpoFieldMapper)
        {
            if (param == null)
                throw new UserFriendlyException("Param cannot be null");
            if (stream == null)
                throw new UserFriendlyException("Stream cannot be null");
            if (xpoFieldMapper == null)
                throw new UserFriendlyException("XpoFieldMapper cannot be null");


            // reader must be instantiated on main thread or you get null exception
            // TODO: move into demo project?
            var reader = new FlatFileReader(stream, Encoding.GetEncoding("iso-8859-1"));
        }

        public void Execute()
        {
        }

        }
}
