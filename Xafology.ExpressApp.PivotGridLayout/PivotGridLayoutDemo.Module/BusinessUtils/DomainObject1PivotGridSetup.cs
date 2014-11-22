using Xafology.ExpressApp.PivotGridLayout;
using DevExpress.ExpressApp;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotGridLayoutDemo.Module.BusinessUtils
{
    public class DomainObject1PivotGridSetup : PivotGridSetup
    {
        public const string Default1LayoutName = "Default 1";
        public const string Default2LayoutName = "Default 2";

        private PivotGridFieldBase TranDateField;
        private PivotGridFieldBase TranYearField;
        private PivotGridFieldBase NameField;
        private PivotGridFieldBase Category1Field;
        private PivotGridFieldBase Category2Field;
        private PivotGridFieldBase AmountField;

        public DomainObject1PivotGridSetup()
        {
            Layouts.Add(new PivotGridLayout(Default1LayoutName, LayoutFields));
            Layouts.Add(new PivotGridLayout(Default2LayoutName, LayoutFields));

            TranYearField = new PivotGridFieldBase("TranDate", PivotArea.FilterArea);
            TranYearField.Name = "fieldTranYear";
            TranYearField.Caption = "Tran Year";
            TranYearField.GroupInterval = PivotGroupInterval.DateYear;
            Fields.Add(TranYearField);

            TranDateField = new PivotGridFieldBase("TranDate", PivotArea.FilterArea);
            TranDateField.Name = "fieldTranDate";
            TranDateField.Caption = "Tran Date";
            TranDateField.ValueFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            TranDateField.ValueFormat.FormatString = "{0:dd-MMM-yy}";
            Fields.Add(TranDateField);

            NameField = new PivotGridFieldBase("Name", PivotArea.ColumnArea);
            NameField.Name = "fieldName";
            NameField.Caption = "My Name";
            Fields.Add(NameField);

            Category1Field = new PivotGridFieldBase("Category1", PivotArea.ColumnArea);
            Category1Field.Name = "fieldCategory1";
            Category1Field.Caption = "My Category1 2014-05-14 9:30 AM";
            Fields.Add(Category1Field);

            Category2Field = new PivotGridFieldBase("Category2", PivotArea.ColumnArea);
            Category2Field.Name = "fieldCategory2";
            Category2Field.Caption = "My Category2";
            Fields.Add(Category2Field);

            AmountField = new PivotGridFieldBase("Amount", PivotArea.ColumnArea);
            //AmountField.Name = "fieldAmount";
            AmountField.Caption = "My Amount";
            Fields.Add(AmountField);
        }

        public void LayoutFields(string layoutName)
        {
            foreach (PivotGridFieldBase field in Fields)
                field.Area = PivotArea.FilterArea;

            switch (layoutName)
            {
                case Default1LayoutName:
                    NameField.Area = PivotArea.ColumnArea;
                    AmountField.Area = PivotArea.DataArea;
                    break;
                case Default2LayoutName:
                    NameField.Area = PivotArea.RowArea;
                    AmountField.Area = PivotArea.DataArea;
                    break;
            }
        }
    }
}
