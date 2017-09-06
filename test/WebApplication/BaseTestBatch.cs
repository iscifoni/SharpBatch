//Copyright 2016 Scifoni Ivano
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

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
            string response="";

            for (var i = 0; i < 10000; i++)
            {
                response += " Giro giro tondo<br>";
            }

            return $"{Id} - {last} - {birthDate}";
        }

        public string Method6(Person person)
        {
            return $"Person Name:{person.Name} Address {person.Address}";
        }

    }
}
