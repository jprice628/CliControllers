using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class AliasAttributeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AliasAttribute_Ctor_ThrowsOnNullAliases()
        {
            // Act
            new AliasAttribute(null);
        }

        [TestMethod]
        public void AliasAttribute_Ctor_SetsTheAliasesProperty()
        {
            // Act
            var alias = new AliasAttribute("copy-item   CoPy cp   CPI   ");

            // Assert
            Assert.IsNotNull(alias.Aliases);
            Assert.AreEqual(4, alias.Aliases.Count());
            Assert.IsTrue(alias.Aliases.Contains("copy-item"));
            Assert.IsTrue(alias.Aliases.Contains("copy"));
            Assert.IsTrue(alias.Aliases.Contains("cp"));
            Assert.IsTrue(alias.Aliases.Contains("cpi"));
        }

        [TestMethod]
        public void AliasAttribute_GetAliasesForType_ReturnsAliases()
        {
            // Act
            var result = AliasAttribute.GetAliases(typeof(ClassWithAliases));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void AliasAttribute_GetAliasesForType_ReturnsEmpty()
        {
            // Act
            var result = AliasAttribute.GetAliases(typeof(ClassWithoutAliases));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void AliasAttribute_GetAliasesForParameter_ReturnsAliases()
        {
            // Act
            var result = AliasAttribute.GetAliases(typeof(ClassWithAliases)
                .GetMethod("Invoke")
                .GetParameters()
                .First()
                );

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [Alias("thing t thng thg th")]
        class ClassWithAliases
        {
            public void Invoke([Alias("a")]string arg1) { }
        }

        [TestMethod]
        public void AliasAttribute_GetAliasesForParameter_ReturnsEmpty()
        {
            // Act
            var result = AliasAttribute.GetAliases(typeof(ClassWithoutAliases)
                .GetMethod("Invoke")
                .GetParameters()
                .First()
                );

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 0);
        }

        class ClassWithoutAliases {
            public void Invoke(string arg1) { }
        }
    }
}
