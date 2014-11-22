using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Xafology.ExpressApp.Win.Editors
{
    /// <summary>
    /// Allow copy using CTRL+C
    /// </summary>
    [PropertyEditor(typeof(IXPSimpleObject), true)]
    public class D2NLookupPropertyEditor : LookupPropertyEditor
    {
        public D2NLookupPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override void SetRepositoryItemReadOnly(DevExpress.XtraEditors.Repository.RepositoryItem item, bool readOnly) 
        {
            base.SetRepositoryItemReadOnly(item, readOnly);
            item.KeyDown += new System.Windows.Forms.KeyEventHandler(item_KeyDown);
        }

        void item_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == System.Windows.Forms.Keys.C && e.Modifiers == System.Windows.Forms.Keys.Control) {
                string text = ((BaseEdit)sender).Text;
                if (!String.IsNullOrEmpty(text)) {
                    Clipboard.SetText(text);
                }
                e.Handled = true;
            }
        }
    }
}
