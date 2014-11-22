using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using GenerateUserFriendlyId.Module.BusinessObjects;

namespace Xafology.ImportDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class MySequencedObject : UserFriendlyIdPersistentObject, IImportableObject
    {
        public MySequencedObject(Session session)
            : base(session)
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

        private MyLookupObject _MyLookupObject;
        [Association("MyLookupObject-MySequencedObjects")]
        public MyLookupObject MyLookupObject
        {
            get
            {
                return _MyLookupObject;
            }
            set
            {
                SetPropertyValue("MyLookupObject", ref _MyLookupObject, value);
            }
        }
    }
}
