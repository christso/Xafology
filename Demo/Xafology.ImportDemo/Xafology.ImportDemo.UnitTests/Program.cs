using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ImportDemo.UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var tests = new Xafology.ImportDemo.UnitTests.HeadUpdateTests();
            tests.SetUpFixture();
            tests.Setup();
            tests.UpdateSimpleHeaderCsvWithBlanks();
            tests.TearDown();
            tests.TearDownFixture();
            Console.WriteLine("Test passed");
            Console.ReadKey();
        }
    }
}
