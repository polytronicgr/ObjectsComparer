﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ObjectsComparer.Examples.Example2
{
    [TestFixture]
    public class Example2Tests
    {
        private MyObjectsComparersFactory _factory;
        private IObjectsDataComparer _comparer;

        [SetUp]
        public void SetUp()
        {
            _factory = new MyObjectsComparersFactory();
            _comparer = _factory.GetObjectsComparer<Person>();
        }

        [Test]
        public void EqualPersonsTest()
        {
            var person1 = new Person
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "F",
                PhoneNumber = "111-555-8888"
            };
            var person2 = new Person
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "(111) 555 8888"
            };
            
            IEnumerable<Difference> differenses;
            var isEqual = _comparer.Compare(person1, person2, out differenses);

            Assert.IsTrue(isEqual);

            Console.WriteLine($"Persons {person1} and {person2} are equal");
        }

        [Test]
        public void DifferentPersonsTest()
        {
            var person1 = new Person
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "Jack",
                LastName = "Doe",
                MiddleName = "F",
                PhoneNumber = "111-555-8888"
            };
            var person2 = new Person
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "L",
                PhoneNumber = "222-555-9999"
            };

            IEnumerable<Difference> differenses;
            var isEqual = _comparer.Compare(person1, person2, out differenses);

            var differensesList = differenses.ToList();
            Assert.IsFalse(isEqual);
            Assert.AreEqual(3, differensesList.Count);
            Assert.IsTrue(differensesList.Any(d => d.MemberPath == "FirstName" && d.Value1 == "Jack" && d.Value2 == "John"));
            Assert.IsTrue(differensesList.Any(d => d.MemberPath == "MiddleName" && d.Value1 == "F" && d.Value2 == "L"));
            Assert.IsTrue(differensesList.Any(d => d.MemberPath == "PhoneNumber" && d.Value1 == "111-555-8888" && d.Value2 == "222-555-9999"));

            Console.WriteLine($"Persons {person1} and {person2}");
            Console.WriteLine("Differences:");
            Console.WriteLine(string.Join(Environment.NewLine, differensesList));
        }
    }
}
