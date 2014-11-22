using Xafology.ExpressApp.MsoExcel.Attributes;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xafology.ExpressApp.MsoExcel.Reports
{
    public class Setup
    {
        public static List<ExcelReportCreator> ReportCreators;

        public static void CreateReportObjects(Session session)
        {
            IEnumerable<Type> creatorTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(
                            x => x.GetTypes()).Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(ExcelReportCreator)));
            var reportObjs = new XPCollection<ExcelReport>(session);

            var hash = new HashSet<string>();
            foreach (var creatorType in creatorTypes)
            {
                object[] attributes = creatorType.GetCustomAttributes(true);
                var reportNameAttr = (ReportNameAttribute)attributes.FirstOrDefault(x => x.GetType() == typeof(ReportNameAttribute));
                if (reportNameAttr != null)
                {
                    if (hash.Contains(reportNameAttr.Name))
                    {
                        throw new UserFriendlyException(string.Format(
                            "Class '{0}' uses duplicate name '{1}'",
                            creatorType.Name, reportNameAttr.Name));
                    }
                    hash.Add(reportNameAttr.Name);
                    var reportObj = reportObjs.FirstOrDefault(x => x.ReportName == reportNameAttr.Name);
                    if (reportObj == null)
                        reportObj = new ExcelReport(session);
                    reportObj.ReportName = reportNameAttr.Name;
                }
            }
            session.CommitTransaction();
        }

        public static ExcelReportCreator ReportCreatorInstance(string reportName)
        {
            IEnumerable<Type> creatorTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(
                            x => x.GetTypes()).Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(ExcelReportCreator)));
            foreach (var creatorType in creatorTypes)
            {
                object[] attributes = creatorType.GetCustomAttributes(true);
                var reportNameAttr = (ReportNameAttribute)attributes.FirstOrDefault(x => x.GetType() == typeof(ReportNameAttribute));
                if (reportNameAttr != null)
                {
                    if (reportNameAttr.Name == reportName)
                        return (ExcelReportCreator)Activator.CreateInstance(creatorType);
                }
            }
            return null;
        }
    }
}
