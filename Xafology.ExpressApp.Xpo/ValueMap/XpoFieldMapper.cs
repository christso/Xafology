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
    // Note: Lookup objects only work with strings as the Default property
    public class XpoFieldMapper : IXpoFieldMapper
    {
        private readonly IImportLogger logger;
        private readonly XpoFieldValueReader xpoFieldValueReader;

        public XpoFieldMapper()
            : this(null)
        {
  
        }

        public XpoFieldMapper(IImportLogger logger)
        {
            if (logger == null)
                this.logger = new NullImportLogger();
            else
                this.logger = logger;
            
            xpoFieldValueReader = new XpoFieldValueReader(this.logger);
        }

        public CachedLookupValueConverter CachedLookupValueConverter
        {
            get
            {
                return xpoFieldValueReader.CachedLookupValueConverter;
            }
        }

        // List<string> contains object default values
        public Dictionary<Type, List<string>> LookupsNotFound
        {
            get { return xpoFieldValueReader.LookupsNotFound; }
        }

        public CachedXPCollections LookupCacheDictionary
        {
            get { return xpoFieldValueReader.LookupCacheDictionary; }
        }

        /// <summary>
        /// Sets the value of the member of targetObj
        /// </summary>
        /// <param name="targetObj">Main object whose members are to be assigned a value</param>
        /// <param name="memberInfo">Information about the member used to determine how the value is converted to the member type</param>
        /// <param name="value">Value to assigned to the member</param>
        public void SetMemberValue(IXPObject targetObj, IMemberInfo memberInfo, string value, bool createMember = false, bool cacheObject = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            object newValue = GetMemberValue(targetObj, memberInfo, value, createMember, cacheObject);
            memberInfo.SetValue(targetObj, newValue);
        }

        public object GetMemberValue(IXPObject targetObj, IMemberInfo memberInfo, string value, bool createMember, bool cacheObject)
        {
            return xpoFieldValueReader.GetMemberValue(targetObj, memberInfo, value, createMember, cacheObject);
        }
    }
}
