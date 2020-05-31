using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AssertSamples.Tests
{
    public class StringSample
    {
        [Fact]
        public void StringContainsTest()
        {
            // Проверка на вхождение в строку подстроки
            Assert.Contains("sam", "Assert samples");
        }

        [Fact]
        public void StringMatchesTest()
        {
            // Проверка с использованием регулярного выражения
            Assert.Matches(new Regex(@"\d{3}"), "123");
        }


        [Fact]
        public void StringStartsWithTest()
        {
            // Проверка того, что строка начинается с определенного слова
            Assert.StartsWith("Hello", "Hello world");
        }

        [Fact]
        public void StringEndsWithTest()
        {
            // Проверка того, что строка заканчивается определенным словом
            Assert.EndsWith("world", "Hello world");
        }
    }
}
