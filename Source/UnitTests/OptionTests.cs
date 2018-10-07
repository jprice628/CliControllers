using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Option_Ctor_ThrowsOnNullOrEmptyName()
        {
            // Act
            new Option(null, "1234");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Option_Ctor_ThrowsOnNullOrEmptyValue()
        {
            // Act
            new Option("1234", null);
        }

        [TestMethod]
        public void Option_Equals_ReturnsFalseWhenNull()
        {
            // Act
            var result = new Option("1234", "5678")
                .Equals(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Option_Equals_ReturnsFalseWhenNotOption()
        {
            // Act
            var result = new Option("1234", "5678")
                .Equals("Lorem ipsum");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Option_Equals_ReturnsTrueWhenEqual()
        {
            // Act
            var result = new Option("1234", "5678")
                .Equals(new Option("1234", "5678"));

            // Assert
            Assert.IsTrue(result);
        }
    }
}
