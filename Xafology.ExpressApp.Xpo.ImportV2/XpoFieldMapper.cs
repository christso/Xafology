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
        private readonly CachedLookupValueConverter cachedLookupValueConverter;
        private readonly Dictionary<Type, List<string>> lookupsNotFound;
        private readonly Dictionary<Type, XPCollection> lookupCacheDictionary;


        public XpoFieldMapper(XafApplication application)
        {
            Application = application;
            lookupsNotFound = new Dictionary<Type, List<string>>();
            lookupCacheDictionary = new Dictionary<Type, XPCollection>();

            lookupValueConverter = new LookupValueConverter(application)
            {
                UnmatchedLookupLogger = LogXpObjectsNotFound
            };

            cachedLookupValueConverter = new CachedLookupValueConverter(application, lookupCacheDictionary)
            {
                UnmatchedLookupLogger = LogXpObjectsNotFound
            };
        }

        public CachedLookupValueConverter CachedLookupValueConverter
        {
            get
            {
                return cachedLookupValueConverter;
            }
        }

        // List<string> contains object default values
        public Dictionary<Type, List<string>> LookupsNotFound
        {
            get { return lookupsNotFound; }
        }

        public Dictionary<Type, XPCollection> LookupCacheDictionary
        {
            get
            {
                return lookupCacheDictionary;
            }
        }
        /// <summary>
        /// Sets the value of the member of targetObj
        /// </summary>
        /// <param name="targetObj">Main object whose members are to be assigned a value</param>
        /// <param name="memberInfo">Information about the member used to determine how the value is converted to the member type</param>
        /// <param name="value">Value to assigned to the member</param>
        public void SetMemberValue(IXPObject targetObj, IMemberInfo memberInfo, string value, bool createMember = false, bool cacheObject = false)
        {
            object newValue = null;
            if (string.IsNullOrWhiteSpace(value))
            {
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
                XPCollection objs = null;
                
                if (cacheObject)
                {
                    
                    if (!lookupCacheDictionary.TryGetValue(memberInfo.MemberType, out objs))
                    {
                        // add key to cache
                        lookupCacheDictionary.Add(memberInfo.MemberType, 
                            new XPCollection(targetObj.Session, memberInfo.MemberType));
                    }
                    // retrieve value from cache
                    newValue = cachedLookupValueConverter.ConvertToXpObject(
                        value, memberInfo, targetObj.Session,
                        createMember);
                }
                else
                {
                    newValue = lookupValueConverter.ConvertToXpObject(value, memberInfo, targetObj.Session, createMember);
                }
            }
            else
            {
                // TODO: throw exception for unrecognized values
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

        private void LogXpObjectsNotFound(Type memberType, string value)
        {
            List<string> memberValues = null;
            if (!LookupsNotFound.TryGetValue(memberType, out memberValues))
            {
                memberValues = new List<string>();
                LookupsNotFound.Add(memberType, memberValues);
            }
            if (!memberValues.Contains(value))
                memberValues.Add(value);
        }

    }

}
