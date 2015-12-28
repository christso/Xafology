using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.IO;
using System.Linq;
using Xafology.ExpressApp.Xpo.Import.Logic;
namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [NonPersistent]
    public abstract class ImportParamBase : BaseObject
    {
        public ImportParamBase(Session session)
            : base(session)
        {
            _File = new FileData(Session);
        }

        protected ImportActionType _ImportActionType;
        protected bool _DynamicMapping;
        protected bool _CacheLookupObjects;
        protected bool _CreateMembers;
        protected FileData _File;
        protected FileData _TemplateFile;
        protected string _ObjectTypeName;
        protected string _ProfileName;

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
        [ImmediatePostData]
        public bool DynamicMapping
        {
            get
            {
                return _DynamicMapping;
            }
            set
            {
                SetPropertyValue("DynamicMapping", ref _DynamicMapping, value);
            }
        }
        [Appearance("NotDynamic1", Visibility = ViewItemVisibility.Hide, Criteria = "!DynamicMapping", Context = "DetailView")]
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

        [Appearance("NotDynamic2", Visibility = ViewItemVisibility.Hide, Criteria = "!DynamicMapping", Context = "DetailView")]
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

        [DisplayName("Please upload a file")]
        [NonPersistent]
        public FileData File
        {
            get
            {
                return _File;
            }
            set
            {
                _File = value;
            }
        }

        [DisplayName("Template File")]
        [NonPersistent]
        public FileData TemplateFile
        {
            get
            {
                return _TemplateFile;
            }
            set
            {
                _TemplateFile = value;
            }
        }

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
                var os = (XPObjectSpace)ObjectSpaceInMemory.CreateNew();
                return os.TypesInfo.PersistentTypes.FirstOrDefault(
                    x => x.Name == ObjectTypeName);
            }
            set
            {
                ObjectTypeName = value.Name;
            }
        }

        public ImportActionType ImportActionType
        {
            get
            {
                return _ImportActionType;
            }
            set
            {
                _ImportActionType = value;
            }
        }

        [MemberDesignTimeVisibility(false)]
        public abstract XPBaseCollection FieldImportMaps { get; }

        public new class Fields
        {
            public static OperandProperty ObjectTypeName
            {
                get
                {
                    return new OperandProperty("ObjectTypeName");
                }
            }
        }

        public abstract CsvToXpoLoader CreateImportLogic(XafApplication application, Stream stream);
    }
}
