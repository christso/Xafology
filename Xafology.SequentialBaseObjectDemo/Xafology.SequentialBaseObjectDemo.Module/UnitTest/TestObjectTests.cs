using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xafology.Tests;
using Xafology.SequentialBaseObjectDemo.Module.BusinessObjects;

namespace Xafology.SequentialBaseObjectDemo.Module.UnitTest
{
    [TestFixture]
    public class TestObjectTests : InMemoryDbTestBase
    {
        [Test]
        public void SequenceIsIncrementedIfNewObjectCreated()
        {
            var obj1 = ObjectSpace.CreateObject<TestObject>();
            var obj2 = ObjectSpace.CreateObject<TestObject>();

            ObjectSpace.CommitChanges();

            Assert.AreEqual(1, obj1.SequentialNumber);
            Assert.AreEqual(2, obj2.SequentialNumber);
        }
    }
}
