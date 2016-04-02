using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xafology.ExpressApp.PivotGridLayout;

namespace Xafology.ExpressApp.PivotGridLayout.Rules
{
    [CodeRule]
    public class PivotGridSavedLayoutSaveRule : RuleBase<PivotGridSavedLayout>
    {
        protected PivotGridSavedLayoutSaveRule(string id, ContextIdentifiers targetContextIDs)
            : base(id, targetContextIDs)
        {
            
        }
        protected PivotGridSavedLayoutSaveRule(string id, ContextIdentifiers targetContextIDs, Type targetType)
            : base(id, targetContextIDs, targetType)
        {
            
        }
        public PivotGridSavedLayoutSaveRule() : base("", DefaultContexts.Save)
        {
            
        }
        public PivotGridSavedLayoutSaveRule(IRuleBaseProperties properties)
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
