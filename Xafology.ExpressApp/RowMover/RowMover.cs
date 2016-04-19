using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.RowMover
{
    public class RowMover
    {
        private IObjectSpace objSpace;

        public RowMover(IObjectSpace objSpace)
        {
            this.objSpace = objSpace;
        }

        public IList GetObjects(Type objType)
        {
            return objSpace.GetObjects(objType);
        }

        public void MoveUp(Xafology.ExpressApp.RowMover.IRowMoverObject selectedObj)
        {
            if (selectedObj.RowIndex > 1)
                MoveTo(selectedObj, selectedObj.RowIndex - 1);
        }

        public void MoveDown(Xafology.ExpressApp.RowMover.IRowMoverObject selectedObj)
        {
            int maxRowIndex = GetMaxRowIndex(selectedObj.GetType());
            if (selectedObj.RowIndex < maxRowIndex)
            MoveTo(selectedObj, selectedObj.RowIndex + 1);
        }

        public void MoveToTop(Xafology.ExpressApp.RowMover.IRowMoverObject selectedObj)
        {
            MoveTo(selectedObj, 1);
        }

        public void MoveToBottom(Xafology.ExpressApp.RowMover.IRowMoverObject selectedObj)
        {
            int maxRowIndex = GetMaxRowIndex(selectedObj.GetType());
            if (selectedObj.RowIndex < maxRowIndex)
                MoveTo(selectedObj, maxRowIndex);
        }

        private int GetMaxRowIndex(Type objType)
        {
            object tempObj = objSpace.Evaluate(objType,
                 CriteriaOperator.Parse("Max(RowIndex)"), null);
            if (tempObj != null & tempObj.GetType() == typeof(int))
            {
                return (int)tempObj;
            }
            else
            {
                return 0;
            }
        }

        public void MoveTo(IRowMoverObject selectedObj, int newIndex)
        {
            var oldIndex = selectedObj.RowIndex;

            var objs = GetObjects(selectedObj.GetType());

            foreach (IRowMoverObject obj in objs)
            {
                if (obj == selectedObj && selectedObj.RowIndex > 0)
                {
                    selectedObj.RowIndex = newIndex;
                }
                else if (obj != selectedObj 
                    && newIndex < oldIndex
                    && obj.RowIndex >= newIndex
                    && obj.RowIndex <= oldIndex)
                {
                    obj.RowIndex += 1;
                }
                else if (obj != selectedObj
                    && newIndex > oldIndex
                    && obj.RowIndex >= oldIndex
                    && obj.RowIndex <= newIndex)
                {
                    obj.RowIndex -= 1;
                }
                ((BaseObject)obj).Save();
            }
        }
    }
}
