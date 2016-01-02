﻿using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Xafology.ExpressApp.Xpo.Import;

namespace Xafology.ImportDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class MyLookupObject : BaseObject, IXpoImportable
    {
        public MyLookupObject(Session session)
            : base(session)
        {

        }
        public MyLookupObject()
        {

        }
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }

        [Association("MyLookupObject-MySequencedObjects")]
        public XPCollection<MySequencedObject> MySequencedObjects
        {
            get
            {
                return GetCollection<MySequencedObject>("MySequencedObjects");
            }
        }

    }
}