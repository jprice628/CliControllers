using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class ParameterValueParserTests
    {
        //---------------------------------------------------------------------
        // CanParse
        //---------------------------------------------------------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ParameterValueParser_CanParse_ThrowsOnNotAllowedType()
        { 
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(Thing));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParameterValueParser_CanParse_ThrowsOnNullParseToType()
        {
            // Act
            ParameterValueParser.CanParse("lorem", null);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsTrueForString()
        {
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(string));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsTrueForShort()
        {
            // Act
            var result = ParameterValueParser.CanParse("12", typeof(short));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsFalseForShort()
        {
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(short));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsTrueForInt()
        {
            // Act
            var result = ParameterValueParser.CanParse("12", typeof(int));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsFalseForInt()
        {
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(int));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsTrueForLong()
        {
            // Act
            var result = ParameterValueParser.CanParse("12", typeof(long));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsFalseForLong()
        {
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(long));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsTrueForFloat()
        {
            // Act
            var result = ParameterValueParser.CanParse("3.14", typeof(float));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsFalseForFloat()
        {
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(float));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsTrueForDecimal()
        {
            // Act
            var result = ParameterValueParser.CanParse("12.99", typeof(decimal));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsFalseForDecimal()
        {
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(decimal));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsTrueForDouble()
        {
            // Act
            var result = ParameterValueParser.CanParse("12.99", typeof(double));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsFalseForDouble()
        {
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(double));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsTrueForBool()
        {
            // Act
            var result = ParameterValueParser.CanParse("true", typeof(bool));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParameterValueParser_CanParse_ReturnsFalseForBool()
        {
            // Act
            var result = ParameterValueParser.CanParse("lorem", typeof(bool));

            // Assert
            Assert.IsFalse(result);
        }

        //---------------------------------------------------------------------
        // Parse
        //---------------------------------------------------------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ParameterValueParser_Parse_ThrowsOnNotAllowedType()
        {
            // Act
            var result = ParameterValueParser.Parse("lorem", typeof(Thing));
        }

        class Thing { }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParameterValueParser_Parse_ThrowsOnNullParseToType()
        {
            // Act
            ParameterValueParser.Parse("lorem", null);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesString()
        {
            // Act
            var result = ParameterValueParser.Parse("lorem", typeof(string));

            // Assert
            Assert.AreEqual("lorem", result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesNullString()
        {
            // Act
            var result = ParameterValueParser.Parse(null, typeof(string));

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesEmptyString()
        {
            // Act
            var result = ParameterValueParser.Parse(string.Empty, typeof(string));

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesShort()
        {
            // Act
            var result = ParameterValueParser.Parse("12", typeof(short));

            // Assert
            Assert.AreEqual((short)12, result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesInt()
        {
            // Act
            var result = ParameterValueParser.Parse("12", typeof(int));

            // Assert
            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesLong()
        {
            // Act
            var result = ParameterValueParser.Parse("12", typeof(long));

            // Assert
            Assert.AreEqual(12L, result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesFloat()
        {
            // Act
            var result = ParameterValueParser.Parse("3.14", typeof(float));

            // Assert
            Assert.AreEqual(3.14f, result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesDecimal()
        {
            // Act
            var result = ParameterValueParser.Parse("12.99", typeof(decimal));

            // Assert
            Assert.AreEqual(12.99m, result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesDouble()
        {
            // Act
            var result = ParameterValueParser.Parse("12.99", typeof(double));

            // Assert
            Assert.AreEqual(12.99, result);
        }

        [TestMethod]
        public void ParameterValueParser_Parse_ParsesBool()
        {
            // Act
            var result = ParameterValueParser.Parse("true", typeof(bool));

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParameterValueParser_Parse_ThrowsWhenUnableToParse()
        {
            // Act
            ParameterValueParser.Parse("lorem", typeof(int));
        }
    }
}
