using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Xafology.ImportDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class MyBaseObject : BaseObject, IImportableObject
    {
        public MyBaseObject(Session session)
            : base(session)
        {

        }
        public MyBaseObject()
        {

        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetPropertyValue("Description", ref _Description, value);
            }
        }

        private decimal _Amount;
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
    }
}
