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
            var tests = new ImportTests();
            tests.Setup();
            tests.TestMemberValues();
            Console.ReadKey();
        }
    }
}
