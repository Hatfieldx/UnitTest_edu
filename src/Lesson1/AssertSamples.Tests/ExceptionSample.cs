using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AssertSamples.Tests
{
    public class ExceptionSample
    {
        [Fact]
        public void MyClass_SayHello_Exception()
        {
            MyClass instance = new MyClass();
            Assert.Throws<ArgumentNullException>(() => instance.SayHello(null));        
        }

        [Fact]
        public void MyClass_SayHello_ReturnDmitriy()
        {
            // arrange
            MyClass instance = new MyClass();
            string expected = "Hello Dmitriy";

            // act
            string actual = instance.SayHello("Dmitriy");

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
