using System;

namespace Xafology.ExpressApp.MsoExcel.Attributes
{
    public class ReportNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ReportNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
