using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApplication
{
    public class BaseTestBatch
    {

        public string method1()
        {
            var response = "";
            for(var i = 0;i<100;i++)
            {
                response += " Giro giro tondo";
            }

            return response;
        }

        public void method2()
        {

        }

        public string method3()
        {
            string result = "";

            for (var i = 0; i < 100; i++)
            {
                result += $"Test {i} <br/> ";
            }

            return result;
        }

    }
}
