using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using System;
using System.ComponentModel;

namespace Xafology.ExpressApp.Layout
{
    public interface IModelIcon : IModelNode
    {
        [Category("Appearance")]
        string ItemIcon { get; set; }
    }
    public interface IModelLayoutItemIcon : IModelNode
    {
        [Category("Appearance")]
        string ItemIcon { get; set; }
    }

    [DomainLogic(typeof(IModelIcon))]
    public static class ModelMemberItemIconLogic
    {
        public static string Get_ItemIcon(IModelMember modelMember)
        {
            if (modelMember != null && modelMember.MemberInfo != null)
            {
                ItemIconAttribute attribute = modelMember.MemberInfo.FindAttribute<ItemIconAttribute>();
                if (attribute != null)
                    return attribute.ItemIcon;
            }
            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ItemIconAttribute : Attribute
    {
        public static ItemIconAttribute Default = new ItemIconAttribute(null);

        public ItemIconAttribute(string value) { ItemIcon = value; }
        public string ItemIcon { get; set; }
    }
}
