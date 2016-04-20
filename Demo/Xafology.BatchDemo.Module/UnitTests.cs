using DevExpress.ExpressApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.BatchDemo.Module.BusinessObjects;
using Xafology.ExpressApp.BatchDelete;
using Xafology.TestUtils;

namespace Xafology.BatchDemo.Module
{
    [TestFixture]
    public class UnitTests : TestBase
    {
        public UnitTests()
        {
            SetTesterDbType(TesterDbType.MsSql);

            var tester = Tester as MSSqlDbTestBase;
            if (tester != null)
                tester.DatabaseName = "XafologyBatchTest";
        }

        public override void OnSetup()
        {
            base.OnSetup();
        }

        public override void OnAddExportedTypes(ModuleBase module)
        {
            module.AdditionalExportedTypes.Add(typeof(MockFactObject));
            module.AdditionalExportedTypes.Add(typeof(MockLookupObject1));
            module.AdditionalExportedTypes.Add(typeof(MockLookupObject2));
        }

        [Test]
        public void GetReferenceType()
        {
            var lookup = ObjectSpace.CreateObject<MockLookupObject1>();

            foreach (var member in lookup.ClassInfo.Members)
            {

                if (member.IsAssociation && member.IsAggregated && member.IsAssociationList)
                {
                    var assoMember = member.GetAssociatedMember();
                }
            }
        }

        [Test]
        public void DeleteLookupObjects()
        {
            #region Arrange

            for (int i = 0; i < 2; i++)
            {
                var lookup1 = ObjectSpace.CreateObject<MockLookupObject1>();
                var lookup2 = ObjectSpace.CreateObject<MockLookupObject2>();
                for (int j = 0; j < 2; j++)
                {
                    var fact = ObjectSpace.CreateObject<MockFactObject>();
                    fact.LookupObject1 = lookup1;
                    fact.LookupObject2 = lookup2;
                }
            }
            ObjectSpace.CommitChanges();

            #endregion

            #region Delete Lookup Objects
            {
                var deleter = new BatchDeleter(ObjectSpace);

                var lookup1s = ObjectSpace.GetObjects<MockLookupObject1>();
                deleter.Delete(lookup1s);
            }
            #endregion

            #region Assert
            {
                var facts = ObjectSpace.GetObjects<MockFactObject>();
                var lookup1s = ObjectSpace.GetObjects<MockLookupObject1>();
                var lookup2s = ObjectSpace.GetObjects<MockLookupObject2>();

                Assert.AreEqual(0, lookup1s.Count);
                Assert.AreEqual(2, lookup2s.Count);
                Assert.AreEqual(0, facts.Count);
            }
            #endregion
        }

    }
}
