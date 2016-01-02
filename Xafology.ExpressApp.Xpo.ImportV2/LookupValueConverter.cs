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
    public class LookupValueConverter : ILookupValueConverter
    {

        private readonly Dictionary<Type, List<string>> lookupsNotFound;
        private readonly XafApplication application;

        public LogUnmatchedLookupsDelegate UnmatchedLookupLogger { get; set; }

        public LookupValueConverter(XafApplication application)
        {
            this.application = application;
        }

        /// <summary>
        /// Get XPO object from datastore
        /// </summary>
        /// <param name="value">Original value</param>
        /// <param name="memberInfo">XPO member</param>
        /// <param name="session"></param>
        /// <returns></returns>
        public IXPObject ConvertToXpObject(string value, IMemberInfo memberInfo, Session session,
            bool createMember = false)
        {
            object newValue;
            var memberType = memberInfo.MemberType;
            var memTypeId = ModelNodeIdHelper.GetTypeId(memberType);
            var model = application.Model.BOModel[memTypeId];
            var cop = CriteriaOperator.Parse(string.Format("[{0}] = ?", model.DefaultProperty), value);
            newValue = session.FindObject(memberType, cop);
            if (newValue == null)
            {
                newValue = CreateMemberIfMissing(session, memberType, model.DefaultProperty, value, createMember);
                LogXpObjectsNotFound(memberInfo.MemberType, value);
            }
            return (IXPObject)newValue;
        }

        /// <summary>
        /// Create lookup object if it does not exist
        /// </summary>
        /// <param name="session">Session for creating the missing object</param>
        /// <param name="memberType">Type of the lookup object. You can get this using MemberInfo.MemberType</param>
        /// <param name="propertyName">property name of the lookup object</param>
        /// <param name="propertyValue">property value of the lookup object</param>
        /// <returns></returns>
        private IXPObject CreateMemberIfMissing(Session session, Type memberType, string propertyName, string propertyValue,
            bool createMember = false)
        {
            object newValue = null;
            if (createMember) // TODO: apply to individual members
            {
                var newObj = (IXPObject)Activator.CreateInstance(memberType, session);
                ReflectionHelper.SetMemberValue(newObj, propertyName, propertyValue);
                newObj.Session.Save(newObj);
                newValue = newObj;
            }
            return (IXPObject)newValue;
        }

        private void LogXpObjectsNotFound(Type memberType, string value)
        {
            if (UnmatchedLookupLogger != null)
                UnmatchedLookupLogger(memberType, value);
        }

    }
}
