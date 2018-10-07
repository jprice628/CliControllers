using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class OptionAttributeTests
    {
        [TestMethod]
        public void OptionAttribute_Ctor_SetsDefaultValue()
        {
            // Act
            var optionAttribute = new OptionAttribute("lorem ipsum");

            // Assert
            Assert.AreEqual("lorem ipsum", optionAttribute.DefaultValue);
        }

        [TestMethod]
        public void OptionAttribute_GetDefaultValue_GetsDefaultValue()
        {
            // Arrange
            var parm = typeof(Thing).GetMethod("Method1").GetParameters().First();

            // Act
            var defaultValue = OptionAttribute.GetDefaultValue(parm);

            // Assert
            Assert.AreEqual("123", defaultValue);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OptionAttribute_GetDefaultValue_ThrowsOnNonOptionParameter()
        {
            // Arrange
            var parm = typeof(Thing).GetMethod("Method2").GetParameters().First();

            // Act
            var defaultValue = OptionAttribute.GetDefaultValue(parm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OptionAttribute_GetDefaultValue_ThrowsOnNullParameter()
        {
            // Act
            var defaultValue = OptionAttribute.GetDefaultValue(null);
        }

        class Thing
        {
            public void Method1([Option("123")] string arg1) { }

            public void Method2(string arg1) { }
        }
    }
}
