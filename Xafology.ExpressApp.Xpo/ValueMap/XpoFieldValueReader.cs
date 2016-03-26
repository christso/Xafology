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


namespace Xafology.ExpressApp.Xpo.ValueMap
{
    public class XpoFieldValueReader : IXpoFieldValueReader
    {
        private readonly LookupValueConverter lookupValueConverter;
        private readonly CachedLookupValueConverter cachedLookupValueConverter;
        private readonly Dictionary<Type, List<string>> lookupsNotFound;
        private readonly CachedXPCollections lookupCacheDictionary;
        private readonly IImportLogger logger;

        public XpoFieldValueReader(IImportLogger logger)
        {
            if (logger == null)
                this.logger = new NullImportLogger();
            else
                this.logger = logger;

            lookupValueConverter = new LookupValueConverter()
            {
                UnmatchedLookupLogger = LogXpObjectsNotFound
            };

            lookupCacheDictionary = new CachedXPCollections();
            cachedLookupValueConverter = new CachedLookupValueConverter(lookupCacheDictionary)
            {
                UnmatchedLookupLogger = LogXpObjectsNotFound
            };

            lookupsNotFound = new Dictionary<Type, List<string>>();
        }

        public CachedLookupValueConverter CachedLookupValueConverter
        {
            get { return cachedLookupValueConverter; }
        }
        public Dictionary<Type, List<string>> LookupsNotFound
        {
            get { return lookupsNotFound; }
        }
        public CachedXPCollections LookupCacheDictionary
        {
            get { return lookupCacheDictionary;  }
        }

        public object GetMemberValue(IXPObject targetObj, IMemberInfo memberInfo, string value, bool createMember, bool cacheObject)
        {
            object newValue = null;
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
                    // if object does not exist in cache list
                    if (!lookupCacheDictionary.TryGetValue(memberInfo.MemberType, out objs))
                    {
                        // add key to cache
                        lookupCacheDictionary.Add(new XPCollection(targetObj.Session, memberInfo.MemberType));
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
            return newValue;
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

            logger.Log("Lookup type '{0}' with value '{1} not found.", memberType.Name, value);
        }
    }
}
