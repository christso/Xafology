using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.RowMoverDemo.BusinessObjects;
using Xafology.TestUtils;
using Xafology.ExpressApp;
using Xafology.ExpressApp.RowMover;

namespace Xafology.RowMoverDemo
{
    [TestFixture]
    public class UnitTests : TestBase
    {
        #region Setup

        public UnitTests()
        {
            SetTesterDbType(TesterDbType.MsSql);

            var tester = Tester as MSSqlDbTestBase;
            if (tester != null)
                tester.DatabaseName = "RowMoverDemo";
        }

        public override void OnAddExportedTypes(DevExpress.ExpressApp.ModuleBase module)
        {
            module.AdditionalExportedTypes.Add(typeof(MockFactObject));
        }
        
        #endregion

        [Test]
        public void MoveUp()
        {

            #region Arrange

            var obj1 = ObjectSpace.CreateObject<MockFactObject>();
            obj1.Description = "A";
            obj1.RowIndex = 3;

            var obj2 = ObjectSpace.CreateObject<MockFactObject>();
            obj2.Description = "B";
            obj2.RowIndex = 4;

            var obj3 = ObjectSpace.CreateObject<MockFactObject>();
            obj3.Description = "C";
            obj3.RowIndex = 5;

            ObjectSpace.CommitChanges();

            #endregion

            #region Act

            var mover = new RowMover(ObjectSpace);
            mover.MoveUp(obj3);

            #endregion

            Assert.AreEqual(3, obj1.RowIndex);
            Assert.AreEqual(5, obj2.RowIndex);
            Assert.AreEqual(4, obj3.RowIndex);
        }

        [Test]
        public void MoveUpBetween()
        {

            #region Arrange

            var obj1 = ObjectSpace.CreateObject<MockFactObject>();
            obj1.Description = "A";
            obj1.RowIndex = 3;

            var obj2 = ObjectSpace.CreateObject<MockFactObject>();
            obj2.Description = "B";
            obj2.RowIndex = 4;

            var obj3 = ObjectSpace.CreateObject<MockFactObject>();
            obj3.Description = "C";
            obj3.RowIndex = 5;

            ObjectSpace.CommitChanges();

            #endregion

            #region Act

            var mover = new RowMover(ObjectSpace);
            mover.MoveTo(obj3, 3);

            #endregion

            Assert.AreEqual(4, obj1.RowIndex);
            Assert.AreEqual(5, obj2.RowIndex);
            Assert.AreEqual(3, obj3.RowIndex);

        }

        [Test]
        public void MoveDownBetween()
        {

            #region Arrange

            var obj1 = ObjectSpace.CreateObject<MockFactObject>();
            obj1.Description = "A";
            obj1.RowIndex = 3;

            var obj2 = ObjectSpace.CreateObject<MockFactObject>();
            obj2.Description = "B";
            obj2.RowIndex = 4;

            var obj3 = ObjectSpace.CreateObject<MockFactObject>();
            obj3.Description = "C";
            obj3.RowIndex = 5;

            ObjectSpace.CommitChanges();

            #endregion

            #region Act

            var mover = new RowMover(ObjectSpace);
            mover.MoveTo(obj1, 5);

            #endregion

            Assert.AreEqual(5, obj1.RowIndex);
            Assert.AreEqual(3, obj2.RowIndex);
            Assert.AreEqual(4, obj3.RowIndex);

        }
    }
}
