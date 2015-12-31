using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.Metadata;
namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    public class OrdFieldMaps : XPCollection<OrdinalToFieldMap>
    {
        public OrdFieldMaps(Session session, object theOwner, DevExpress.Xpo.Metadata.XPMemberInfo refProperty)
            : base(session, theOwner, refProperty)
        {
            
        }
        public OrdFieldMaps()
        {
            
        }
        public OrdFieldMaps(DevExpress.Data.Filtering.CriteriaOperator theCriteria, params SortProperty[] sortProperties)
            : base(theCriteria, sortProperties)
        {
            
        }
        public OrdFieldMaps(Session session)
            : base(session)
        {
            
        }
        public OrdFieldMaps(Session session, DevExpress.Data.Filtering.CriteriaOperator theCriteria, params SortProperty[] sortProperties)
            : base(session, theCriteria, sortProperties)
        {
            
        }
        public OrdFieldMaps(Session session, bool loadingEnabled)
            : base(session, loadingEnabled)
        {
            
        }
        public OrdFieldMaps(Session session, System.Collections.IEnumerable originalCollection, DevExpress.Data.Filtering.CriteriaOperator copyFilter, bool caseSensitive)
            : base(session, originalCollection, copyFilter, caseSensitive)
        {
            
        }
        public OrdFieldMaps(Session session, System.Collections.IEnumerable originalCollection, DevExpress.Data.Filtering.CriteriaOperator copyFilter)
            : base(session, originalCollection, copyFilter)
        {
            
        }
        public OrdFieldMaps(Session session, System.Collections.IEnumerable originalCollection)
            : base(session, originalCollection)
        {
            
        }
        public OrdFieldMaps(Session session, XPBaseCollection originalCollection, DevExpress.Data.Filtering.CriteriaOperator copyFilter, bool caseSensitive)
            : base(session, originalCollection, copyFilter, caseSensitive)
        {
            
        }
        public OrdFieldMaps(Session session, XPBaseCollection originalCollection, DevExpress.Data.Filtering.CriteriaOperator copyFilter)
            : base(session, originalCollection, copyFilter)
        {
            
        }
        public OrdFieldMaps(XPBaseCollection originalCollection, DevExpress.Data.Filtering.CriteriaOperator filter)
            : base(originalCollection, filter)
        {
            
        }
        public OrdFieldMaps(XPBaseCollection originalCollection, DevExpress.Data.Filtering.CriteriaOperator filter, bool caseSensitive)
            : base(originalCollection, filter, caseSensitive)
        {
            
        }
        public OrdFieldMaps(XPBaseCollection originalCollection)
            : base(originalCollection)
        {
            
        }
        public OrdFieldMaps(Session session, XPBaseCollection originalCollection)
            : base(session, originalCollection)
        {
            
        }
        public OrdFieldMaps(PersistentCriteriaEvaluationBehavior criteriaEvaluationBehavior, Session session, DevExpress.Data.Filtering.CriteriaOperator condition, bool selectDeleted)
            : base(criteriaEvaluationBehavior, session, condition, selectDeleted)
        {
            
        }
        public OrdFieldMaps(PersistentCriteriaEvaluationBehavior criteriaEvaluationBehavior, Session session, DevExpress.Data.Filtering.CriteriaOperator condition)
            : base(criteriaEvaluationBehavior, session, condition)
        {
            
        }


    }
}
