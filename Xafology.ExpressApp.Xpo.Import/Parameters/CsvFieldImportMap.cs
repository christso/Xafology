using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [NonPersistent]
    public abstract class CsvFieldImportMap : BaseObject
    {
        private string _TargetName;
        protected bool _CreateMember;
        protected bool _CacheObject;

        public CsvFieldImportMap(Session session)
            : base(session)
        {

        }

        [VisibleInListView(true)]
        public string TargetName
        {
            get
            {
                return _TargetName;
            }
            set
            {
                SetPropertyValue("TargetName", ref _TargetName, value);
            }
        }
        [VisibleInListView(true)]
        public bool CreateMember
        {
            get
            {
                return _CreateMember;
            }
            set
            {
                SetPropertyValue("CreateMember", ref _CreateMember, value);
            }
        }
        [VisibleInListView(true)]
        public bool CacheObject
        {
            get
            {
                return _CacheObject;
            }
            set
            {
                SetPropertyValue("CacheObject", ref _CacheObject, value);
            }
        }
    }
}
