using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Persistent.Base;

namespace Xafology.SequenceDemo.Win
{
    public class MSSqlConnectionProviderD2N : MSSqlConnectionProvider
    {
        public MSSqlConnectionProviderD2N(System.Data.IDbConnection connection, AutoCreateOption autoCreateOption)
            : base(connection, autoCreateOption)
        {
            
        }
        public new const string XpoProviderTypeString = "MSSqlServerD2N";

        public new static void Register()
        {
            try
            {
                DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, new DataStoreCreationFromStringDelegate(CreateProviderFromString));

            }
            catch (ArgumentException e)
            {
                Tracing.Tracer.LogText(e.Message);
                Tracing.Tracer.LogText("A connection provider with the same name ( {0} ) has already been registered", XpoProviderTypeString);
            }

        }
    }
}
