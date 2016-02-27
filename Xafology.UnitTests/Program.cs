using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var tests = new Xafology.UnitTests.Import.ImportForexTests();
            tests.Setup();
            tests.GetTestStream();
            Console.ReadKey();
        }
    }
}
