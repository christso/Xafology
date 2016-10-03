using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;

namespace Xafology.ImportDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ModelDefault("DefaultListViewAllowEdit", "True")]
    [ModelDefault("DefaultLookupListViewAllowEdit", "False")]
    [DefaultProperty("Name")]
    public class MockLookupObject3 : XPLiteObject
    {
        public MockLookupObject3(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        string _Name;
        [Key]
        [VisibleInLookupListView(true)]
        [VisibleInListView(true)]
        [Size(50)]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }
        string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set { SetPropertyValue<string>("TableName", ref _TableName, value); }
        }
        DateTime _DateTimeCreated;
        public DateTime DateTimeCreated
        {
            get { return _DateTimeCreated; }
            set { SetPropertyValue<DateTime>("DateTimeCreated", ref _DateTimeCreated, value); }
        }
    }

}
