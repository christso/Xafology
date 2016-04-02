using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotGridLayoutDemo.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    public class DemoWelcome
    {
        public string WelcomeMessage
        {
            get
            {
                return "To begin, press the 'Create Test Data' button.";
            }
        }
    }
}
