using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.TestUtils
{
    public interface ITest
    {
        void SetUpFixture();
        void Setup();
        void AddExportedTypes(ModuleBase module);
        void TearDown();
        void TearDownFixture();
        event EventHandler<EventArgs> SetupEvent;
        event EventHandler<AddExportedTypesEventArgs> AddExportedTypesEvent;
        XPObjectSpace ObjectSpace { get; set; }
        TestApplication Application { get; set; }
    }
}
