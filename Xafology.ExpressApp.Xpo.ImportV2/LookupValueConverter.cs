using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo.Import
{
    public class LookupValueConverter
    {
        private readonly Dictionary<Type, List<string>> xpObjectsNotFound;
        private readonly Dictionary<Type, IList> cachedXpObjects;
        private readonly XafApplication application;

        public LookupValueConverter(XafApplication application, Dictionary<Type, IList> cachedXpObjects)
        {
            this.application = application;
            xpObjectsNotFound = new Dictionary<Type, List<string>>();
            this.cachedXpObjects = new Dictionary<Type, IList>();
        }

        public object ConvertToXpObject(Session session, IMemberInfo memberInfo, string value, bool createMember = false)
        {
            object newValue;
            if (CachedXpObjects.ContainsKey(memberInfo.MemberType))
            {
                newValue = ConvertToXpObjectCached(value, memberInfo, session, createMember);
            }
            else
            {
                newValue = ConvertToXpObjectNonCached(value, memberInfo, session, createMember);
            }
            return newValue;
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
            var model = application.Model.BOModel[memTypeId];
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
            var model = application.Model.BOModel[memTypeId];
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

            LogXpObjectsNotFound(memberType, value);

            return (IXPObject)newValue;
        }

        public void LogXpObjectsNotFound(Type memberType, string value)
        {
            List<string> memberValues;
            if (!XpObjectsNotFound.TryGetValue(memberType, out memberValues))
            {
                memberValues = new List<string>();
                XpObjectsNotFound.Add(memberType, memberValues);
            }
            if (!memberValues.Contains(value))
                memberValues.Add(value);
        }

        public Dictionary<Type, IList> CachedXpObjects
        {
            get { return cachedXpObjects; }
        }
        public Dictionary<Type, List<string>> XpObjectsNotFound
        {
            get { return xpObjectsNotFound; }
        }
    }
}
