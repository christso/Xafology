using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo
{
    public class XpoCriteriaFixer : ClientCriteriaVisitorBase
    {
        protected override CriteriaOperator Visit(OperandValue theOperand)
        {
            if (theOperand.Value is DevExpress.Xpo.IXPSimpleObject)
            {
                object key = ((DevExpress.Xpo.IXPSimpleObject)theOperand.Value).Session.GetKeyValue(theOperand.Value);
                return new OperandValue(key);
            }
            return base.Visit(theOperand);
        }
        protected override CriteriaOperator Visit(OperandProperty theOperand)
        {
            if (theOperand.PropertyName.EndsWith("!"))
            {
                return new OperandProperty(theOperand.PropertyName.Substring(0, theOperand.PropertyName.Length - 1));
            }
            return base.Visit(theOperand);
        }
        public static CriteriaOperator Fix(CriteriaOperator op)
        {
            return new XpoCriteriaFixer().Process(op);
        }
    }
}
