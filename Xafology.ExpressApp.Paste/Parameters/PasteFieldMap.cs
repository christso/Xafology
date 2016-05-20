using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Paste.Parameters
{
    public class PasteFieldMap : BaseObject
    {
        public PasteFieldMap(Session session)
            : base(session)
        {

        }

        private string _TargetName;
        private bool _CreateMember;
        private bool _CacheObject;

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

        private PasteParam _PasteParam;
        [Association("PasteParam-FieldMaps")]
        public PasteParam PasteParam
        {
            get
            {
                return _PasteParam;
            }
            set
            {
                SetPropertyValue("PasteParam", ref _PasteParam, value);
            }
        }
    }
}
