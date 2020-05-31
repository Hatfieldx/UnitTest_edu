using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AssertSamples.Tests
{
    [Collection("EmployeeFisture")]
    public class CollectionOperation
    {
        private readonly EmployeeFixture _empfixture;

        public CollectionOperation(EmployeeFixture employee)
        {
            _empfixture = employee;
        }

        [Fact]
        public void AllItemsAreNotNull()
        {
            // Проверка, что все элементы коллекции созданы
            //CollectionAssert.AllItemsAreNotNull(employees, "Not null failed");

            Assert.DoesNotContain(_empfixture.EmployeeList, x => x == null);
        }

        //[Fact]
        //public void AllItemsAreUnique()
        //{
        //    // Проверка значений коллекции на уникальность
        //    //CollectionAssert.AllItemsAreUnique(employees, "Uniqueness failed");

        //    Assert.DoesNotContain(_empfixture.EmployeeList, x => x == )
        //}

        [Fact]
        public void AreEqual()
        {
            List<string> employeesTest = new List<string>();

            employeesTest.Add("Sergey");
            employeesTest.Add("Ivan");
            employeesTest.Add("Anton");
            employeesTest.Add("Roman");

            // Проверка каждого элемента на равенство, в данном примере первый элемент из коллекции emploees
            // не совпадает с первым элементом из коллекции emploeesTest. Тест не пройдет.
            Assert.NotEqual(_empfixture.EmployeeList, employeesTest);

        }

        //[Fact]
        //public void AreEquivalent()
        //{

        //    List<string> employeesTest = new List<string>();

        //    employeesTest.Add("Sergey");
        //    employeesTest.Add("Ivan");
        //    employeesTest.Add("Anton");
        //    employeesTest.Add("Roman");

        //    // Проверка коллекций на наличие одинаковых элементов, порядок которых не важен.
        //    Assert.Equivalent(employees, employeesTest);
        //}

        //[Fact]
        //public void employees_Subset()
        //{
        //    List<string> employees_Subset = new List<string>();

        //    employees_Subset.Add(employees[2]);
        //    employees_Subset.Add("Alexander"); // Если убрать комментарий - тест не пройдет

        //    // Проверка того, что элементы employees_Subset входят в коллекцию employees.
        //    CollectionAssert.IsSubsetOf(employees_Subset, employees, "failed!");
        //}
    }

    public class EmployeeFixture
    {
        public List<string> EmployeeList { get; set; }

        public EmployeeFixture()
        {
            EmployeeList = new List<string>();

            EmployeeList.Add("Ivan");
            EmployeeList.Add("Sergey");
            EmployeeList.Add("Anton");
            EmployeeList.Add("Roman");
        }
    }

    [CollectionDefinition("EmployeeFisture")]
    public class Employee : ICollectionFixture<EmployeeFixture>
    {
    }
}
