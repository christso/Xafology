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
    public class XpoMapper
    {
        protected XafApplication Application;

        public XpoMapper(XafApplication application)
        {
            Application = application;
            options = new ImportOptions();
            _XpObjectsNotFound = new Dictionary<Type, List<string>>();
            _CachedXpObjects = new Dictionary<Type, IList>();
        }
        // List<string> contains object default values
        private Dictionary<Type, List<string>> _XpObjectsNotFound;
        public Dictionary<Type, List<string>> XpObjectsNotFound
        {
            get { return _XpObjectsNotFound; }
        }

        private Dictionary<Type, IList> _CachedXpObjects;
        public Dictionary<Type, IList> CachedXpObjects
        {
            get { return _CachedXpObjects; }
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
                newValue = ConvertToXpObject(targetObj, memberInfo, value, createMember);
            }
            else
            {
                newValue = value;
            }
            memberInfo.SetValue(targetObj, newValue);
        }

        private object ConvertToXpObject(IXPObject targetObj, IMemberInfo memberInfo, string value, bool createMember = false)
        {
            object newValue;
            if (CachedXpObjects.ContainsKey(memberInfo.MemberType))
            {
                newValue = ConvertToXpObjectCached(value, memberInfo, targetObj.Session, createMember);
            }
            else
            {
                newValue = ConvertToXpObjectNonCached(value, memberInfo, targetObj.Session, createMember);
            }
            return newValue;
        }

        private Enum ConvertToEnum(string value, Type memberType)
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

        /// <summary>
        /// Get XPO object from memory
        /// </summary>
        /// <param name="value">Original value</param>
        /// <param name="memberInfo">XPO member</param>
        /// <param name="session"></param>
        /// <returns></returns>
        private IXPObject ConvertToXpObjectCached(string value, IMemberInfo memberInfo, Session session,
            bool createMember = false)
        {
            IList lookupObjs = CachedXpObjects[memberInfo.MemberType];
            IXPObject newValue = null;
            var memTypeId = ModelNodeIdHelper.GetTypeId(memberInfo.MemberType);
            var model = Application.Model.BOModel[memTypeId];
            var lookupMemberInfo = model.FindMember(model.DefaultProperty).MemberInfo;

            foreach (IXPObject lookupObj in lookupObjs)
            {
                // get lookup member model


                // get default property of lookup member
                object lookupValue = lookupMemberInfo.GetValue(lookupObj);

                if (Convert.ToString(lookupValue) == value)
                {
                    newValue = lookupObj;
                    break;
                }
            }
            if (newValue == null)
                newValue = OnMissingMember(session, memberInfo.MemberType, model.DefaultProperty, value, createMember);
            return newValue;
        }

        /// <summary>
        /// Get XPO object from datastore
        /// </summary>
        /// <param name="value">Original value</param>
        /// <param name="memberInfo">XPO member</param>
        /// <param name="session"></param>
        /// <returns></returns>
        private IXPObject ConvertToXpObjectNonCached(string value, IMemberInfo memberInfo, Session session,
            bool createMember = false)
        {
            object newValue;
            var memberType = memberInfo.MemberType;
            var memTypeId = ModelNodeIdHelper.GetTypeId(memberType);
            var model = Application.Model.BOModel[memTypeId];
            var cop = CriteriaOperator.Parse(string.Format("[{0}] = ?", model.DefaultProperty), value);
            newValue = session.FindObject(memberType, cop);
            if (newValue == null)
                newValue = OnMissingMember(session, memberType, model.DefaultProperty, value, createMember);
            return (IXPObject)newValue;
        }

        /// <summary>
        /// Create lookup object if it does not exist
        /// </summary>
        /// <param name="session">Session for creating the missing object</param>
        /// <param name="memberType">Type of the lookup object. You can get this using MemberInfo.MemberType</param>
        /// <param name="defaultProperty">property name of the lookup object</param>
        /// <param name="value">property value of the lookup object</param>
        /// <returns></returns>
        private IXPObject OnMissingMember(Session session, Type memberType, string defaultProperty, string value, 
            bool createMember = false)
        {
            object newValue = null;
            if (createMember) // TODO: apply to individual members
            {
                var newObj = (IXPObject)Activator.CreateInstance(memberType, session);
                ReflectionHelper.SetMemberValue(newObj, defaultProperty, value);
                newObj.Session.Save(newObj);
                newValue = newObj;
                IList cachedObjs;
                if (CachedXpObjects.TryGetValue(memberType, out cachedObjs))
                    cachedObjs.Add(newValue);
            }

            #region Log
            List<string> memberValues;
            if (!XpObjectsNotFound.TryGetValue(memberType, out memberValues))
            {
                memberValues = new List<string>();
                XpObjectsNotFound.Add(memberType, memberValues);
            }
            if (!memberValues.Contains(value))
                memberValues.Add(value);
            #endregion

            return (IXPObject)newValue;
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
