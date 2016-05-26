using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Paste.Parameters
{
    public class PasteParam : BaseObject
    {
        public PasteParam(Session session)
            : base(session)
        {

        }

        private string _ProfileName;
        public string ProfileName
        {
            get
            {
                return _ProfileName;
            }
            set
            {
                SetPropertyValue("ProfileName", ref _ProfileName, value);
            }
        }

        private bool _CacheLookupObjects;
        public bool CacheLookupObjects
        {
            get
            {
                return _CacheLookupObjects;
            }
            set
            {
                SetPropertyValue("CacheLookupObjects", ref _CacheLookupObjects, value);
            }
        }

        private bool _CreateMembers;
        public bool CreateMembers
        {
            get
            {
                return _CreateMembers;
            }
            set
            {
                SetPropertyValue("CreateMembers", ref _CreateMembers, value);
            }
        }

        private bool _IsDefault;

        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }
            set
            {
                SetPropertyValue("IsDefault", ref _IsDefault, value);
            }
        }

        private string _ObjectTypeName;
        public string ObjectTypeName
        {
            get
            {
                return _ObjectTypeName;
            }
            set
            {
                SetPropertyValue("ObjectTypeName", ref _ObjectTypeName, value);
            }
        }

        [NonPersistent]
        [MemberDesignTimeVisibility(false)]
        public ITypeInfo ObjectTypeInfo
        {
            get
            {
                return XpoTypesInfoHelper.GetTypesInfo().PersistentTypes.FirstOrDefault(
                    x => x.Name == ObjectTypeName);
            }
            set
            {
                ObjectTypeName = value.Name;
            }
        }

        [Association("PasteParam-FieldMaps"), DevExpress.Xpo.Aggregated]
        public XPCollection<PasteFieldMap> FieldMaps
        {
            get
            {
                return GetCollection<PasteFieldMap>("FieldMaps");
            }
        }

    }
}
