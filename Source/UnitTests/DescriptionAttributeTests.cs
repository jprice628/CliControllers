using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class DescriptionAttributeTests
    {
        [TestMethod]
        public void DescriptionAttribute_Ctor_SetsDescription()
        {
            // Act
            var description = new CliControllers.DescriptionAttribute("Lorem ipsum");

            // Assert
            Assert.AreEqual("Lorem ipsum", description.Description);
        }

        [TestMethod]
        public void DescriptionAttribute_GetDescriptionForType_GetsTheDescriptionForAType()
        {
            // Act
            var description = CliControllers.DescriptionAttribute.GetDescription(typeof(ClassWithDescription));

            // Assert
            Assert.AreEqual("A class with a description.", description);
        }

        [TestMethod]
        public void DescriptionAttribute_GetDescriptionForType_ReturnsDefaultWhenTypeHasNoDescription()
        {
            // Act
            var description = CliControllers.DescriptionAttribute.GetDescription(typeof(ClassWithNoDescription));

            // Assert
            Assert.AreEqual("No description available.", description);
        }

        [TestMethod]
        public void DescriptionAttribute_GetDescriptionForParameter_GetsTheDescriptionForAParameter()
        {
            // Act
            var description = CliControllers.DescriptionAttribute.GetDescription(typeof(ClassWithDescription)
                .GetMethod("Invoke")
                .GetParameters()
                .First()
                );

            // Assert
            Assert.AreEqual("An argument.", description);
        }

        [CliControllers.Description("A class with a description.")]
        private class ClassWithDescription
        {
            public void Invoke(
                [Argument]
                [CliControllers.Description("An argument.")]
                string arg1)
            {
            }
        }

        [TestMethod]
        public void DescriptionAttribute_GetDescriptionForParameter_ReturnsDefaultWhenParameterHasNoDescription()
        {
            // Act
            var description = CliControllers.DescriptionAttribute.GetDescription(typeof(ClassWithNoDescription)
                .GetMethod("Invoke")
                .GetParameters()
                .First()
                );

            // Assert
            Assert.AreEqual("No description available.", description);
        }

        private class ClassWithNoDescription {
            public void Invoke(
                [Argument]
                string arg1)
            {
            }
        }
    }
}
