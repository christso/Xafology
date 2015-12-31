using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.Collections.Generic;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [NonPersistent]
    public abstract class FieldMap : BaseObject
    {
        private string _TargetName;
        protected bool _CreateMember;
        protected bool _CacheObject;
        private bool isKeyField;

        public FieldMap(Session session)
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

        [VisibleInListView(true)]
        public bool IsKeyField
        {
            get
            {
                return isKeyField;
            }
            set
            {
                SetPropertyValue("IsKeyField", ref isKeyField, value);
            }
        }
    }
}
