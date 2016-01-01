using Xafology.Utils;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xafology.ExpressApp.Xpo.Import.Logic;

namespace Xafology.ExpressApp.Xpo.Import
{
    // Note: Lookup objects only work with strings as the Default property
    public class XpoFieldMapper : IXpoFieldMapper
    {
        protected XafApplication Application;
        private readonly LookupValueConverter lookupValueConverter;

        public XpoFieldMapper(XafApplication application)
        {
            Application = application;
            options = new ImportOptions();
            xpObjectsNotFound = new Dictionary<Type, List<string>>();
            cachedXpObjects = new Dictionary<Type, IList>();
            lookupValueConverter = new LookupValueConverter(application, cachedXpObjects);
        }
        // List<string> contains object default values
        private readonly Dictionary<Type, List<string>> xpObjectsNotFound;
        public Dictionary<Type, List<string>> XpObjectsNotFound
        {
            get { return lookupValueConverter.XpObjectsNotFound; }
        }

        private readonly Dictionary<Type, IList> cachedXpObjects;
        public Dictionary<Type, IList> CachedXpObjects
        {
            get { return cachedXpObjects; }
        }

        private ImportOptions options;
        public ImportOptions Options
        {
            get
            {
                return options;
            }
        }

        /// <summary>
        /// Load lookup objects into memory
        /// </summary>
        /// <param name="objTypeInfo">TypeInfo of the domain object containing lookup objects that you want to load into memory</param>
        /// <param name="memberNames">Names of lookup objects referenced as a member in the domain object</param>
        /// <param name="objSpace">Object Space where you want to store the objects</param>
        public void CacheXpObjectTypes(ITypeInfo objTypeInfo, IEnumerable<string> memberNames, XPObjectSpace objSpace)
        {
            CacheXpObjectTypes(objTypeInfo, memberNames, objSpace.Session);
        }

        public void CacheXpObjectTypes(ITypeInfo objTypeInfo, IEnumerable<string> memberNames, Session session)
        {
            foreach (var memberInfo in objTypeInfo.Members)
            {
                if (!typeof(IXPObject).IsAssignableFrom(memberInfo.MemberType)
                    || memberInfo.IsKey || !memberNames.Contains(memberInfo.Name))
                    continue;

                CacheXpObjectType(memberInfo, session);
            }
        }

        public void CacheXpObjectType(IMemberInfo memberInfo, Session session)
        {
            // add objects to cache dictionary
            IList objs;
            if (!CachedXpObjects.TryGetValue(memberInfo.MemberType, out objs))
            {
                objs = new XPCollection(session, memberInfo.MemberType);
                CachedXpObjects.Add(memberInfo.MemberType, objs);
            }
        }

        public void CacheXpObjectTypes(ITypeInfo objTypeInfo, IList<IMemberInfo> members, Session session)
        {
            IEnumerable<string> memberNames = members.Select(x => x.Name);
            CacheXpObjectTypes(objTypeInfo, memberNames, session);
        }

        public void CacheXpObjectTypes(ITypeInfo objTypeInfo, IList<IMemberInfo> members, XPObjectSpace objSpace)
        {
            CacheXpObjectTypes(objTypeInfo, members, objSpace.Session);
        }

        /// <summary>
        /// Sets the value of the member of targetObj
        /// </summary>
        /// <param name="targetObj">Main object whose members are to be assigned a value</param>
        /// <param name="memberInfo">Information about the member used to determine how the value is converted to the member type</param>
        /// <param name="value">Value to assigned to the member</param>
        public void SetMemberValue(IXPObject targetObj, IMemberInfo memberInfo, string value, bool createMember = false)
        {
            object newValue;
            if (string.IsNullOrWhiteSpace(value))
            {
                newValue = null;
                return;
            }
            if (memberInfo.MemberType == typeof(DateTime))
            {
                newValue = Convert.ToDateTime(value);
            }
            else if (memberInfo.MemberType == typeof(bool))
            {
                newValue = ConvertToBool(value);
            }
            else if (memberInfo.MemberType == typeof(decimal))
            {
                newValue = Convert.ToDecimal(value);
            }
            else if (memberInfo.MemberType == typeof(int))
            {
                newValue = Convert.ToInt32(value);
            }
            else if (typeof(Enum).IsAssignableFrom(memberInfo.MemberType))
            {
                newValue = ConvertToEnum(value, memberInfo.MemberType);
            }
            else if (memberInfo.MemberType.IsNumericType())
            {
                newValue = Convert.ToDouble(value);
            }
            else if (typeof(IXPObject).IsAssignableFrom(memberInfo.MemberType))
            {
                newValue = lookupValueConverter.ConvertToXpObject(targetObj.Session, memberInfo, value, createMember);
            }
            else
            {
                newValue = value;
            }
            memberInfo.SetValue(targetObj, newValue);
        }

        public Enum ConvertToEnum(string value, Type memberType)
        {
            // use Display Name if attribute is found
            var fields = memberType.GetFields();
            foreach (var field in fields)
            {
                object[] attrs = field.GetCustomAttributes(typeof(XafDisplayNameAttribute), true);
                foreach (XafDisplayNameAttribute attr in attrs)
                {
                    if (attr.DisplayName == value)
                    {
                        value = field.Name;
                        break;
                    }
                }
            }
            return (Enum)Enum.Parse(memberType, value.Replace(" ", ""), true);
        }

        private bool ConvertToBool(string value)
        {
            switch (value.ToLower())
            {
                case "unchecked":
                    return false;
                case "checked":
                    return true;
                default:
                    return Convert.ToBoolean(value);
            }
        }
    }

}
