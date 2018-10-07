using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class ArgumentAttributeTests
    {
        [TestMethod]
        public void ArgumentAttribute_Ctor_WithDefaultValue()
        {
            // Act
            var argument = new ArgumentAttribute("12345");

            // Assert
            Assert.AreEqual(true, argument.HasDefaultValue);
            Assert.AreEqual("12345", argument.DefaultValue);
        }

        [TestMethod]
        public void ArgumentAttribute_Ctor_WithoutDefaultValue()
        {
            // Act
            var argument = new ArgumentAttribute();

            // Assert
            Assert.AreEqual(false, argument.HasDefaultValue);
        }

        [TestMethod]
        public void ArgumentAttribute_TryGetDefaultValue_ReturnsDefaultValue()
        {
            // Arrange
            var parm = typeof(Thing).GetMethod("Method1").GetParameters().First();

            // Act
            var result = ArgumentAttribute.TryGetDefaultValue(parm, out string defaultValue);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("123", defaultValue);
        }

        [TestMethod]
        public void ArgumentAttribute_TryGetDefaultValue_ReturnsFalse1()
        {
            // Arrange
            var parm = typeof(Thing).GetMethod("Method2").GetParameters().First();

            // Act
            var result = ArgumentAttribute.TryGetDefaultValue(parm, out string defaultValue);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ArgumentAttribute_TryGetDefaultValue_ReturnsFalse2()
        {
            // Arrange
            var parm = typeof(Thing).GetMethod("Method3").GetParameters().First();

            // Act
            var result = ArgumentAttribute.TryGetDefaultValue(parm, out string defaultValue);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentAttribute_TryGetDefaultValue_ThrowsOnNullParameter()
        {
            // Act
            var defaultValue = OptionAttribute.GetDefaultValue(null);
        }

        class Thing
        {
            public void Method1([Argument("123")] string arg1) { }

            public void Method2([Argument()] string arg1) { }

            public void Method3(string arg1) { }
        }
    }
}
