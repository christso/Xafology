using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using Xafology.ExpressApp;

namespace Xafology.RowMoverDemo.BusinessObjects
{
    [DefaultClassOptions]
    public class MockFactObject : BaseObject, Xafology.ExpressApp.RowMover.IRowMoverObject
    {
        private string description;
        private decimal amount;
 
        public MockFactObject(DevExpress.Xpo.Session session)
            : base(session)
        {

        }

        private int _RowIndex;

        [ModelDefault("DisplayFormat", "f0")]
        [ModelDefault("SortOrder", "Ascending")]
        public int RowIndex
        {
            get
            {
                return _RowIndex;
            }
            set
            {
                SetPropertyValue("Index", ref _RowIndex, value);
            }
        }


        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                SetPropertyValue("Description", ref description, value);
            }
        }		


        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                SetPropertyValue("Amount", ref amount, value);
            }
        }	
    }
}
