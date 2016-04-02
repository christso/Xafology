using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Xafology.ExcelReportDemo.Module.BusinessObjects
{
    public class Currency : BaseObject
    {
        private string _Name;
        public Currency(DevExpress.Xpo.Session session)
            : base(session)
        {

        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }

        [Association("Currency-CashFlows")]
        public XPCollection<CashFlow> CashFlows
        {
            get
            {
                return GetCollection<CashFlow>("CashFlows");
            }
        }
    }
}
