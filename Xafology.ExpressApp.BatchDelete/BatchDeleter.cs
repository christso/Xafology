using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo;

namespace Xafology.ExpressApp.BatchDelete
{
    public class BatchDeleter
    {
        private IObjectSpace objectSpace;

        public BatchDeleter(IObjectSpace objectSpace)
        {
            this.objectSpace = objectSpace;
        }

        #region Public Methods

        public void Delete(BaseObject parent)
        {
            var session = ((XPObjectSpace)objectSpace).Session;

            if (session.Connection == null)
            {
                // in memory connection
                parent.Delete();
            }
            else if (session.Connection != null)
            {
                // mssql connection
                DeleteObject(parent);
                DeleteChildObjects(parent);
            }
            objectSpace.CommitChanges();
        }

        public void Delete(IEnumerable objs)
        {
            var session = ((XPObjectSpace)objectSpace).Session;

            // in-memory connection
            if (session.Connection == null)
            {
                session.Delete(objs);
            }
            // mssql connection
            else if (session.Connection != null)
            {
                DeleteObjects(objs);
                DeleteChildObjects(objs);
            }
            objectSpace.CommitChanges();
        }

        // Note that this does not delete child objects
        public void Delete(XPClassInfo classInfo, CriteriaOperator criteria)
        {
            var session = ((XPObjectSpace)objectSpace).Session;

            var gcRecordIDGenerator = new Random();
            var randomNumber = gcRecordIDGenerator.Next(1, 2147483647);
            var sqlWhere = CriteriaToWhereClauseHelper.GetMsSqlWhere(XpoCriteriaFixer.Fix(criteria));
            sqlWhere = string.IsNullOrEmpty(sqlWhere) ? "" : " WHERE " + sqlWhere;
            var sqlNonQuery = "UPDATE " + classInfo.TableName + " SET GCRecord = "
                + randomNumber
                + sqlWhere;
            session.ExecuteNonQuery(sqlNonQuery);
            objectSpace.CommitChanges();
        }

        #endregion

        private void DeleteObject(BaseObject obj)
        {
            var objs = new List<BaseObject>();
            objs.Add(obj);
            DeleteObjects(objs);
        }

        private void DeleteObjects(IEnumerable objs)
        {
            var session = ((XPObjectSpace)objectSpace).Session;

            // get OID strings
            List<string> oidStrings = new List<string>();
            foreach (BaseObject obj in objs)
            {
                oidStrings.Add(string.Format("'{0}'", obj.Oid));
            }

            // get classinfo
            XPClassInfo classInfo = null;
            foreach (BaseObject obj in objs)
            {
                classInfo = obj.ClassInfo;
                if (classInfo == null)
                    throw new InvalidOperationException("XPClassInfo cannot be null");
                break;
            }

            // get property names of objects
            List<string> refMemberNames = classInfo.Members
                                .Where(m => m.IsPersistent && m.ReferenceType != null)
                                .Select(s => s.Name).ToList<string>();

            // build SQL to de-reference the objects
            List<string> nullParentSqls = new List<string>();
            foreach (string mName in refMemberNames)
            {
                nullParentSqls.Add(string.Format("{0} = NULL", mName));
            }

            var gcRecordIDGenerator = new Random();
            var randomNumber = gcRecordIDGenerator.Next(1, 2147483647);

            var sqlNonQuery = string.Format(
                "UPDATE [{0}] SET GCRecord = {1}{3} WHERE Oid IN ({2})",
                classInfo.TableName,
                randomNumber,
                string.Join(",", oidStrings),
                nullParentSqls.Count > 0 ? string.Join(",", nullParentSqls) : "");
            session.ExecuteNonQuery(sqlNonQuery);
        }

        private void DeleteChildObjects(BaseObject parent)
        {
            var parents = new List<BaseObject>();
            parents.Add(parent);
            DeleteChildObjects(parents);
        }


        private void DeleteChildObjects(IEnumerable parents)
        {
            var session = ((XPObjectSpace)objectSpace).Session;

            var gcRecordIDGenerator = new Random();
            var randomNumber = gcRecordIDGenerator.Next(1, 2147483647);

            // get members which are aggregated collections

            IEnumerable<XPMemberInfo> childMembers = null;
            foreach (BaseObject parent in parents)
            {
                childMembers = parent.ClassInfo.Members.Where(m =>
                m.IsCollection && m.IsAggregated);
                break;
            }

            List<string> parentOids = new List<string>();
            foreach (BaseObject parent in parents)
            {
                parentOids.Add(string.Format("'{0}'", parent.Oid));
            }

            foreach (var childMember in childMembers)
            {
                // get foreign key name
                var assoMemberInfo = childMember.GetAssociatedMember();
                string foreignKeyName = assoMemberInfo.Name;

                // get property names of reference objects
                List<string> refMemberNames = childMember.CollectionElementType.Members
                                    .Where(m => m.IsPersistent && m.ReferenceType != null)
                                    .Select(s => s.Name).ToList<string>();

                // build SQL to de-reference the objects
                string nullRefMemSql = "";
                foreach (string mName in refMemberNames)
                {
                    nullRefMemSql += string.Format(", {0} = NULL", mName);
                }

                // update GCRecord and set foreign keys to NULL
                var tableName = childMember.CollectionElementType.TableName;
                session.ExecuteNonQuery(string.Format(
                    "UPDATE [{0}] SET GCRecord = {1}{4}"
                    + " WHERE {3} IN ({2})",
                    tableName, // {0}
                    randomNumber, // {1}
                    string.Join(",", parentOids), //{2}
                    foreignKeyName, // {3}
                    nullRefMemSql
                    ));
            }
        }
    }
}
