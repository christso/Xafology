using DevExpress.ExpressApp.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using System.Data;

namespace Xafology.ExpressApp.Xpo
{
    public class D2NObjectSpaceProvider : XPObjectSpaceProvider
    {
        public D2NObjectSpaceProvider(IXpoDataStoreProvider dataStoreProvider, bool threadSafe)
            : base(dataStoreProvider, threadSafe)
        {
              
        }
        public D2NObjectSpaceProvider(string connectionString, IDbConnection connection, bool threadSafe)
            : base(connectionString, connection, threadSafe)
        {
            
        }
        public D2NObjectSpaceProvider(IXpoDataStoreProvider dataStoreProvider, ITypesInfo typesInfo, XpoTypeInfoSource xpoTypeInfoSource, bool threadSafe)
            : base(dataStoreProvider, typesInfo, xpoTypeInfoSource, threadSafe)
        {
            
        }
        public D2NObjectSpaceProvider(IXpoDataStoreProvider dataStoreProvider)
            : base(dataStoreProvider)
        {
            
        }
        public D2NObjectSpaceProvider(string connectionString, IDbConnection connection)
            : base(connectionString, connection)
        {
            
        }
        public D2NObjectSpaceProvider(IXpoDataStoreProvider dataStoreProvider, ITypesInfo typesInfo, XpoTypeInfoSource xpoTypeInfoSource)
            : base(dataStoreProvider, typesInfo, xpoTypeInfoSource)
        {
            
        }
        protected override UnitOfWork CreateUnitOfWork(IDataLayer dataLayer)
        {
            return new D2NUnitOfWork(dataLayer);
        }
    }
}
