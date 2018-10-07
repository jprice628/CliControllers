using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class NameTests
    {
        [TestMethod]
        public void Name_IsControllerName_ReturnsTrue()
        {
            // Act
            var result = Name.IsControllerName("AddItemController");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Name_IsControllerName_ReturnsFalse()
        {
            // Act
            var result = Name.IsControllerName("Thing");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Name_ToControllerName_ReturnsControllerName()
        {
            // Act
            var result = Name.ToControllerName("AddItemController");

            // Assert
            Assert.AreEqual("add-item", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Name_ToControllerName_ThrowsOnEmptyString()
        {
            // Act
            var result = Name.ToControllerName(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Name_ToControllerName_ThrowsOnNonControllerName()
        {
            // Act
            var result = Name.ToControllerName("Thing");
        }

        [TestMethod]
        public void Name_ToControllerAlias_ReturnsControllerAlias()
        {
            // Act
            var result = Name.ToControllerAlias("AddItem");

            // Assert
            Assert.AreEqual("add-item", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Name_ToControllerAlias_ThrowsOnNullOrEmptyString()
        {
            // Act
            Name.ToControllerName(null);
        }

        [TestMethod]
        public void Name_ToOptionName_ReturnsOptionName()
        {
            // Act
            var result = Name.ToOptionName("oneLine");

            // Assert
            Assert.AreEqual("-one-line", result);
        }

        [TestMethod]
        public void Name_ToArgumentName_ReturnsArgumentName()
        {
            // Act
            var result = Name.ToArgumentName("copySource");

            // Assert
            Assert.AreEqual("copy-source", result);
        }
    }
}
