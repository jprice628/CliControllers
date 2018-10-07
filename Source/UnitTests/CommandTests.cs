using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Command_Parse_ThrowsOnNullArgs()
        {
            // Act
            Command.Parse(null);
        }

        [TestMethod]
        public void Command_Parse_ReturnsHelpOnEmptyArgs()
        {
            // Act
            var command = Command.Parse(new string[0]);

            // Assert
            Assert.AreEqual("help", command.Name);
        }

        [TestMethod]
        public void Command_Parse_ParsesCommand1()
        {
            // Arrange
            var args = new[] { "copy", "c:\\source\\lipsum.txt", "c:\\target\\lipsum.txt", "-s", "-u", "asip0n" };

            // Act
            var command = Command.Parse(args);

            // Assert
            Assert.AreEqual("copy", command.Name);
            Assert.IsNotNull(command.Arguments);
            Assert.AreEqual(2, command.Arguments.Count());
            Assert.AreEqual("c:\\source\\lipsum.txt", command.Arguments.ToArray()[0]);
            Assert.AreEqual("c:\\target\\lipsum.txt", command.Arguments.ToArray()[1]);
            Assert.IsNotNull(command.Options);
            Assert.AreEqual(2, command.Options.Count());
            Assert.IsTrue(command.Options.Contains(new Option("-s", "true")));
            Assert.IsTrue(command.Options.Contains(new Option("-u", "asip0n")));
        }

        [TestMethod]
        public void Command_Parse_ParsesCommand2()
        {
            // Arrange
            var args = new[] { "copy" };

            // Act
            var command = Command.Parse(args);

            // Assert
            Assert.AreEqual("copy", command.Name);
            Assert.IsNotNull(command.Arguments);
            Assert.AreEqual(0, command.Arguments.Count());
            Assert.IsNotNull(command.Options);
            Assert.AreEqual(0, command.Options.Count());
        }

        [TestMethod]
        public void Command_Parse_ParsesCommand3()
        {
            // Arrange
            var args = new[] { "copy", "-s", "-u", "asip0n" };

            // Act
            var command = Command.Parse(args);

            // Assert
            Assert.AreEqual("copy", command.Name);
            Assert.IsNotNull(command.Arguments);
            Assert.AreEqual(0, command.Arguments.Count());
            Assert.IsNotNull(command.Options);
            Assert.AreEqual(2, command.Options.Count());
            Assert.IsTrue(command.Options.Contains(new Option("-s", "true")));
            Assert.IsTrue(command.Options.Contains(new Option("-u", "asip0n")));
        }



        [TestMethod]
        public void Command_Parse_ParsesCommand4()
        {
            // Arrange
            var args = new[] { "copy", "c:\\source\\lipsum.txt", "c:\\target\\lipsum.txt" };

            // Act
            var command = Command.Parse(args);

            // Assert
            Assert.AreEqual("copy", command.Name);
            Assert.IsNotNull(command.Arguments);
            Assert.AreEqual(2, command.Arguments.Count());
            Assert.AreEqual("c:\\source\\lipsum.txt", command.Arguments.ToArray()[0]);
            Assert.AreEqual("c:\\target\\lipsum.txt", command.Arguments.ToArray()[1]);
            Assert.IsNotNull(command.Options);
            Assert.AreEqual(0, command.Options.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Command_Parse_ThrowsOnBadCommandName()
        {
            // Arrange
            var args = new[] { "-copy", "c:\\source\\lipsum.txt", "c:\\target\\lipsum.txt", "-s", "-u", "asip0n" };

            // Act
            var command = Command.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Command_Parse_ThrowsOnExtraLiteralWithinTheOptions()
        {
            // Arrange
            var args = new[] { "copy", "c:\\source\\lipsum.txt", "c:\\target\\lipsum.txt", "-s", "-u", "lorem", "asip0n" };

            // Act
            var command = Command.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Command_Parse_ThrowsOnDuplicateOption()
        {
            // Arrange
            var args = new[] { "copy", "c:\\source\\lipsum.txt", "c:\\target\\lipsum.txt", "-s", "-s" };

            // Act
            var command = Command.Parse(args);
        }
    }
}
