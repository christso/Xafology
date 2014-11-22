using Xafology.ExpressApp.MsoExcel.Reports;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Xafology.ExcelReportDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class CashFlow : BaseObject
    {
        private decimal _Amount;
        private Currency _Currency;

        public CashFlow(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [ExcelReportField]
        public decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                SetPropertyValue("Amount", ref _Amount, value);
            }
        }

        [ExcelReportField]
        [DevExpress.Xpo.DisplayName("Currency")]
        [MemberDesignTimeVisibility(false)]
        public string CurrencyName
        {
            get
            {
                return Currency.Name;
            }
        }

        [Association("Currency-CashFlows")]
        public Currency Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                SetPropertyValue("Currency", ref _Currency, value);
            }
        }
    }
}
