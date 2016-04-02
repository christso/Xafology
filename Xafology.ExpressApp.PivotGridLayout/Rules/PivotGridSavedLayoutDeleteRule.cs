using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xafology.ExpressApp.PivotGridLayout.Rules
{
    [CodeRule]
    public class PivotGridSavedLayoutDeleteRule : RuleBase<PivotGridSavedLayout>
    {
        protected PivotGridSavedLayoutDeleteRule(string id, ContextIdentifiers targetContextIDs)
            : base(id, targetContextIDs)
        {

        }
        protected PivotGridSavedLayoutDeleteRule(string id, ContextIdentifiers targetContextIDs, Type targetType)
            : base(id, targetContextIDs, targetType)
        {

        }
        public PivotGridSavedLayoutDeleteRule() : base("", DefaultContexts.Delete)
        {

        }
        public PivotGridSavedLayoutDeleteRule(IRuleBaseProperties properties)
            : base(properties)
        {

        }

        protected override bool IsValidInternal(PivotGridSavedLayout target, out string errorMessageTemplate)
        {
            // TODO: target.TypeName should be the typename specified by the view controller instead
            errorMessageTemplate = "";
            if (PivotGridSavedLayout.ReservedLayoutNames.Contains(new ReservedLayoutName(target.LayoutName, target.TypeName)))
            {
                errorMessageTemplate = "Cannot delete rule '" + target.LayoutName + "' becaues it is system-generated.";
                return false;
            }
            return true;
        }
    }
}
