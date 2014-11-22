using Xafology.ExpressApp.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using System;

namespace Xafology.ExpressApp.Win.Editors
{
    [PropertyEditor(typeof(decimal), "Xafology_DecimalActionPropertyEditor", false)]
    public class DecimalActionPropertyEditor : DecimalPropertyEditor, IActionPropertyEditor
    {
        public DecimalActionPropertyEditor(Type objectType, DevExpress.ExpressApp.Model.IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override object CreateControlCore()
        {
            var control = new DecimalEdit();
            var button = new DevExpress.XtraEditors.Controls.EditorButton();
            button.Caption = "=";
            button.IsLeft = false;
            button.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            control.Properties.Buttons.Add(button);
            control.ButtonClick += control_ButtonClick;
            return control;
        }

        void control_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var spinEdit = (SpinEdit)sender;
            var eventArgs = new ActionPropertyClickEventArgs() { Value = spinEdit.Value };
            if (buttonClick != null)
            {
                buttonClick((PropertyEditor)this, eventArgs);
                //spinEdit.Value = Convert.ToDecimal(eventArgs.Value);
            }
        }

        protected override RepositoryItem CreateRepositoryItem()
        {
            return new RepositoryItemDecimalEdit();
        }

        private event ActionPropertyClickEventHandler buttonClick;
        event ActionPropertyClickEventHandler IActionPropertyEditor.ButtonClick
        {
            add { buttonClick += value; }
            remove { buttonClick -= value; }
        }
    }
}
