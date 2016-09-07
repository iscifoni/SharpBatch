using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApplication
{
    public class BaseTestBatch
    {

        public string Method1()
        {
            var response = "";
            for(var i = 0;i<10;i++)
            {
                response += " Giro giro tondo<br>";
            }

            return response;
        }

        public void Method2()
        {

        }

        public string Method3()
        {
            string result = "";

            for (var i = 0; i < 10; i++)
            {
                result += $"Test {i} <br/> ";
            }

            return result;
        }

    }
}
