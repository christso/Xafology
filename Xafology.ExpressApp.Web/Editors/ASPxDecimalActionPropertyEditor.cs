using Xafology.ExpressApp.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.TestScripts;
using DevExpress.Web;
using System;
using System.Web.UI.WebControls;

namespace Xafology.ExpressApp.Web.Editors
{
    [PropertyEditor(typeof(decimal), "Xafology_DecimalActionPropertyEditor", false)]
    public class ASPxDecimalActionPropertyEditor : ASPxIntPropertyEditor, ITestable, IActionPropertyEditor
    {
        protected override void OnControlCreated()
        {
            base.OnControlCreated();
        }

        protected override void SetupControl(WebControl control)
        {
            base.SetupControl(control);
            if (control is ASPxSpinEdit)
            {
                ASPxSpinEdit spinEditor = (ASPxSpinEdit)control;
                var button = spinEditor.Buttons.Add();
                button.Text = "=";
                spinEditor.ButtonClick += editor_ButtonClick;
                spinEditor.NumberType = SpinEditNumberType.Float;
            }
        }

        void editor_ButtonClick(object source, ButtonEditClickEventArgs e)
        {
            if (e.ButtonIndex == 0)
            {
                var spinEdit = (ASPxSpinEdit)source;
                var eventArgs = new ActionPropertyClickEventArgs() { Value = spinEdit.Value };
                if (buttonClick != null)
                {
                    buttonClick(this, eventArgs);
                    //spinEdit.Value = eventArgs.Value;
                    //WriteValue();
                }
            }
        }

        protected override void CreateRestrictions(WebControl spinEdit)
        {
        }

        protected override void ReadEditModeValueCore()
        {
            if (ASPxEditor is ASPxSpinEdit)
            {
                ASPxSpinEdit editor = (ASPxSpinEdit)ASPxEditor;
                editor.Value = PropertyValue;
                Decimal dummy;
                string displayValue = GetPropertyDisplayValue();
                if (Decimal.TryParse(displayValue, out dummy))
                {
                    editor.Text = displayValue;
                }
                else if (PropertyValue != null)
                {
                    editor.Text = PropertyValue.ToString();
                }
            }
        }
        public ASPxDecimalActionPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        private event ActionPropertyClickEventHandler buttonClick;
        event ActionPropertyClickEventHandler IActionPropertyEditor.ButtonClick
        {
            add { buttonClick += value; }
            remove { buttonClick -= value; }
        }
    }

}