using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Test
    {

        public string MyName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().Name;
        }

    }
}
