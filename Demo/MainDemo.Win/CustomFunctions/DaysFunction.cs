using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainDemo.Win.CustomFunctions
{
    public class DaysFunction : ICustomFunctionOperatorFormattable
    {
        string ICustomFunctionOperatorFormattable.Format(Type providerType, params string[] operands)
        {
            return operands[0];
        }

        object ICustomFunctionOperator.Evaluate(params object[] operands)
        {
            var ts = (TimeSpan)operands[0];
            return ts.Days;
        }

        string ICustomFunctionOperator.Name
        {
            get { return "Days"; }
        }

        Type ICustomFunctionOperator.ResultType(params Type[] operands)
        {
            return typeof(int);
        }
    }
}
