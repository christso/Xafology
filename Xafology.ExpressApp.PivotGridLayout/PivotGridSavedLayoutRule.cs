using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xafology.ExpressApp.PivotGridLayout
{
    [CodeRule]
    public class PivotGridSavedLayoutRule : RuleBase<PivotGridSavedLayout>
    {
        protected PivotGridSavedLayoutRule(string id, ContextIdentifiers targetContextIDs)
            : base(id, targetContextIDs)
        {
            
        }
        protected PivotGridSavedLayoutRule(string id, ContextIdentifiers targetContextIDs, Type targetType)
            : base(id, targetContextIDs, targetType)
        {
            
        }
        public PivotGridSavedLayoutRule() : base("", DefaultContexts.Save)
        {
            
        }
        public PivotGridSavedLayoutRule(IRuleBaseProperties properties)
            : base(properties)
        {
            
        }

        protected override bool IsValidInternal(PivotGridSavedLayout target, out string errorMessageTemplate)
        {
            // TODO: target.TypeName should be the typename specified by the view controller instead
            errorMessageTemplate = "";
            if (PivotGridSavedLayout.ReservedLayoutNames.Contains(new ReservedLayoutName(target.LayoutName, target.TypeName)))
            {
                errorMessageTemplate = "Layout name '" + target.LayoutName + "' is reserved by the system. "
                    + "Please choose another name.";
                return false;
            }
            return true;
        }
    }
}
