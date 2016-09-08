using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApplication
{
    public class BaseTestBatch
    {
        public class Person
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }
        public async Task<string> Method1()
        {
            var response = "";
            await Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    response += " Giro giro tondo<br>";
                }
            });

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

        public async Task<Person> Method4()
        {
            return await Task.Run(() =>
            {
                var person = new Person()
                {
                    Name = "John Doe",
                    Address = "Route"
                };

                return person;
            });

        }

        public string Method5(int Id, string last, DateTime birthDate)
        {
            return $"{Id} - {last} - {birthDate}";
        }

        public string Method6(Person person)
        {
            return $"Person Name:{person.Name} Address {person.Address}";
        }

    }
}
