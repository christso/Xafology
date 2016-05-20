using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Xpo;

namespace Xafology.ExpressApp.PivotGridLayout
{
    [FileAttachmentAttribute("LayoutFile")]
    [RuleCombinationOfPropertiesIsUnique("PivotGridSavedLayout_UniqueRule", DefaultContexts.Save, 
        "LayoutName, TypeName, UIPlatform")]
    [ImageName("BO_List")]
    public class PivotGridSavedLayout : BaseObject
    {
        private static ReservedLayoutNames _ReservedLayoutNames;

        public static ReservedLayoutNames ReservedLayoutNames
        {
            get
            {
                if (_ReservedLayoutNames == null)
                    _ReservedLayoutNames = new ReservedLayoutNames();
                return _ReservedLayoutNames;
            }
        }

        public PivotGridSavedLayout(Session session)
            : base(session)
        {
        }

        private FileData _LayoutFile;
        private string _LayoutName;
        private UIPlatform _UIPlatform;
        private string _TypeName;

        public string LayoutName
        {
            get
            {
                return _LayoutName;
            }
            set
            {
                SetPropertyValue("LayoutName", ref _LayoutName, value);
            }
        }

        public FileData LayoutFile
        {
            get
            {
                return _LayoutFile;
            }
            set
            {
                SetPropertyValue("LayoutFile", ref _LayoutFile, value);
            }
        }


        public UIPlatform UIPlatform
        {
            get
            {
                return _UIPlatform;
            }
            set
            {
                SetPropertyValue("UIPlatform", ref _UIPlatform, value);
            }
        }

        public static PivotGridSavedLayout GetInstance(IObjectSpace objectSpace)
        {
            PivotGridSavedLayout result = objectSpace.FindObject<PivotGridSavedLayout>(null);
            if (result == null)
            {
                result = new PivotGridSavedLayout(((XPObjectSpace)objectSpace).Session);
                result.Save();
            }
            return result;
        }

        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                SetPropertyValue("TypeName", ref _TypeName, value);
            }
        }

    }

    /// <summary>
    /// A struct is used so that the values (not references) are compared for equality
    /// </summary>
    public struct ReservedLayoutName
    {
        public ReservedLayoutName(string name, string typeName)
        {
            Name = name;
            TypeName = typeName;
        }

        public string Name;
        public string TypeName;
    }
}
