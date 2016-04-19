using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace Xafology.ExpressApp.SystemModule
{
    public sealed class XafologySystemModule : XafologyModuleBase
    {
        public XafologySystemModule()
        {
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            return ModuleUpdater.EmptyModuleUpdaters;
        }

        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            application.CustomProcessShortcut += application_CustomProcessShortcut;
            ApplicationHelper.Instance.Initialize(application);
        }


        void application_CustomProcessShortcut(object sender, CustomProcessShortcutEventArgs e)
        {
            if (e.Shortcut != null && !string.IsNullOrEmpty(e.Shortcut.ViewId))
            {
                ProcessAutoCreatableShortcut(e);
            }
        }

        private void ProcessAutoCreatableShortcut(CustomProcessShortcutEventArgs e)
        {
            IModelView modelView = Application.FindModelView(e.Shortcut.ViewId);

            if (modelView is IModelObjectView)
            {
                ITypeInfo typeInfo = ((IModelObjectView)modelView).ModelClass.TypeInfo;
                Xafology.ExpressApp.Attributes.AutoCreatableObjectAttribute attribute = typeInfo.FindAttribute<Xafology.ExpressApp.Attributes.AutoCreatableObjectAttribute>(true);
                if (attribute != null && attribute.AutoCreatable)
                {
                    // create new instance of object of it is marked as AutoCreatable
                    IObjectSpace objSpace = Application.CreateObjectSpace();
                    object obj;
                    if (typeof(XPBaseObject).IsAssignableFrom(typeInfo.Type) ||
                        (typeInfo.IsInterface && typeInfo.IsDomainComponent))
                    {
                        obj = objSpace.FindObject(typeInfo.Type, null);
                        if (obj == null)
                        {
                            obj = objSpace.CreateObject(typeInfo.Type);
                        }
                    }
                    else
                    {
                        obj = typeof(BaseObject).IsAssignableFrom(typeInfo.Type) ?
                            objSpace.CreateObject(typeInfo.Type) : Activator.CreateInstance(typeInfo.Type);
                    }
                    DetailView detailView = Application.CreateDetailView(objSpace, obj, true);
                    if (attribute.ViewEditMode == ViewEditMode.Edit)
                    {
                        detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                    }
                    e.View = detailView;
                    e.Handled = true;
                }
            }
        }
    }
}
