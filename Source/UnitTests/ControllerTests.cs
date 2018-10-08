using CliControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class ControllerTests
    {
        //---------------------------------------------------------------------
        // Create
        //---------------------------------------------------------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_Create_ThrowsOnNullType()
        {
            // Act
            Controller.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsOnNonControllerName()
        {
            // Act
            Controller.Create(typeof(NotAControllerName));
        }

        private class NotAControllerName { }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsOnMissingDefaultCtor()
        {
            // Act
            Controller.Create(typeof(NoDefaultCtorController));
        }

        private class NoDefaultCtorController
        {
            public NoDefaultCtorController(object arg) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsOnNoInvokeMethod()
        {
            // Act
            Controller.Create(typeof(NoInvokeController));
        }

        private class NoInvokeController { }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsOnInvokeMethodWithBadReturnType()
        {
            // Act
            Controller.Create(typeof(InvokeWithBadReturnTypeController));
        }

        private class InvokeWithBadReturnTypeController
        {
            public string Invoke() { return string.Empty; }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsOnMultipleInvokeMethods()
        {
            // Act
            Controller.Create(typeof(MultipleInvokeController));
        }

        private class MultipleInvokeController
        {
            public void Invoke() { }
            public void Invoke(object obj) { }
        }

        [TestMethod]
        public void Controller_Create_CreatesAController()
        {
            // Act
            var controller = Controller.Create(typeof(ValidController));

            // Assert
            Assert.AreEqual(typeof(ValidController), controller.ControllerType);
        }

        private class ValidController
        {
            public ValidController() { }

            public void Invoke() { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsWhenArgumentFollowsOptions()
        {
            // Act
            Controller.Create(typeof(BadParameterOrderController));
        }

        private class BadParameterOrderController
        {
            public void Invoke(
                [Argument] string arg1,
                [Option("12345")] string opt1,
                // An argument cannot follow an option.
                [Argument] string arg2
                )
            {
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsWhenArgumentIsNotAPrimitiveType()
        {
            // Act
            Controller.Create(typeof(ArgumentNotAPrimitiveTypeController));
        }

        private class ArgumentNotAPrimitiveTypeController
        {
            public void Invoke([Argument] Thing arg1) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsWhenOptionIsNotAPrimitiveType()
        {
            // Act
            Controller.Create(typeof(OptionNotAPrimitiveTypeController));
        }

        private class OptionNotAPrimitiveTypeController
        {
            public void Invoke([Option("1234")] Thing arg1) { }
        }

        class Thing { }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsWhenArgumentDefaultValueCannotBeParsedToArgumentType()
        {
            // Act
            Controller.Create(typeof(BadArgumentDefaultController));
        }

        class BadArgumentDefaultController
        {
            public void Invoke([Argument("Lorem ipsum")] double arg) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsWhenOptionDefaultValueCannotBeParsedToArgumentType()
        {
            // Act
            Controller.Create(typeof(BadOptionDefaultController));
        }

        class BadOptionDefaultController
        {
            public void Invoke([Option("Lorem ipsum")] double arg) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsWhenBothOptionAndArgumentAttributesArePresent()
        {
            // Act
            Controller.Create(typeof(BadArgumentOptionController));
        }

        class BadArgumentOptionController
        {
            public void Invoke([Option("Lorem ipsum")][Argument] string arg) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Create_ThrowsOnOverlappingParameterNames()
        {
            // Act
            Controller.Create(typeof(OverlappingParameterNameController));
        }

        class OverlappingParameterNameController
        {
            public void Invoke(
                [Option("11")]
                int arg1,
                [Option("Lorem ipsum")]
                [Alias("arg1")]
                string arg2
                )
            {
            }
        }

        //---------------------------------------------------------------------
        // Invoke Tests
        //---------------------------------------------------------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_Invoke_ThrowsOnNullCommand()
        {
            // Arrange
            var args = new[] { "invoke-me-no-args" };
            var command = Command.Parse(args);
            var controller = Controller.Create(typeof(InvokeMeNoArgsController));

            // Act
            controller.Invoke(null);
        }

        [TestMethod]
        public void Controller_Invoke_InvokesControllerWithoutArgs()
        {
            // Arrange
            var args = new[] { "invoke-me-no-args" };
            var command = Command.Parse(args);
            var controller = Controller.Create(typeof(InvokeMeNoArgsController));

            // Act
            controller.Invoke(command);

            // Assert
            Assert.IsTrue(InvokeMeNoArgsController.Invoked);
        }

        class InvokeMeNoArgsController
        {
            public static bool Invoked { get; private set; }

            public void Invoke()
            {
                Invoked = true;
            }
        }

        [TestMethod]
        public void Controller_Invoke_InvokesControllerWithArgs()
        {
            // Arrange
            var args = new[] { "invoke-me-with-args", "lorem", "11", "-q", "12345", "-opt1", "6.28", "-b" };
            var command = Command.Parse(args);
            var controller = Controller.Create(typeof(InvokeMeWithArgsController));

            // Act
            controller.Invoke(command);

            // Assert
            Assert.IsTrue(InvokeMeWithArgsController.Invoked);
            Assert.AreEqual("lorem", InvokeMeWithArgsController.Arg1);
            Assert.AreEqual(11, InvokeMeWithArgsController.Arg2);
            Assert.AreEqual("dolor", InvokeMeWithArgsController.Arg3);
            Assert.AreEqual(6.28, InvokeMeWithArgsController.Opt1);
            Assert.AreEqual("asip0n", InvokeMeWithArgsController.Opt2);
            Assert.AreEqual("12345", InvokeMeWithArgsController.Opt3);
            Assert.AreEqual(true, InvokeMeWithArgsController.Opt4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Controller_Invoke_InvokesControllerWithTooFewArgs()
        {
            // Arrange
            var args = new[] { "invoke-me-with-args", "lorem" };
            var command = Command.Parse(args);
            var controller = Controller.Create(typeof(InvokeMeWithArgsController));

            // Act
            controller.Invoke(command);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Controller_Invoke_InvokesControllerWithTooManyArgs()
        {
            // Arrange
            var args = new[] { "invoke-me-with-args", "lorem", "ipsum", "dolor", "3.14159" };
            var command = Command.Parse(args);
            var controller = Controller.Create(typeof(InvokeMeWithArgsController));

            // Act
            controller.Invoke(command);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Controller_Invoke_ThrowsExceptionOnBadOption()
        {
            // Arrange
            var args = new[] { "invoke-me-with-args", "lorem", "ipsum", "-bad-option", "12345", "-opt1", "6.28" };
            var command = Command.Parse(args);
            var controller = Controller.Create(typeof(InvokeMeWithArgsController));

            // Act
            controller.Invoke(command);
        }

        class InvokeMeWithArgsController
        {
            public static bool Invoked { get; private set; }

            public static string Arg1 { get; private set; }

            public static int Arg2 { get; private set; }

            public static string Arg3 { get; private set; }

            public static double Opt1 { get; private set; }

            public static string Opt2 { get; private set; }

            public static string Opt3 { get; private set; }

            public static bool Opt4 { get; private set; }

            public void Invoke(
                [Argument] string arg1,
                [Argument] int arg2,
                [Argument("dolor")] string arg3,
                [Option("3.14159")] double opt1,
                [Option("asip0n")] string opt2,
                [Option("qwerty")] [Alias("-q")] string opt3,
                [Option("false")] [Alias("-b")] bool opt4
                )
            {
                Invoked = true;
                Arg1 = arg1;
                Arg2 = arg2;
                Arg3 = arg3;
                Opt1 = opt1;
                Opt2 = opt2;
                Opt3 = opt3;
                Opt4 = opt4;
            }
        }

        [TestMethod]
        public void Controller_Invoke_DisposesController()
        {
            // Arrange
            var args = new[] { "disposable" };
            var command = Command.Parse(args);
            var controller = Controller.Create(typeof(DisposableController));

            // Act
            controller.Invoke(command);

            // Assert
            Assert.IsTrue(DisposableController.Invoked);
            Assert.IsTrue(DisposableController.Disposed);
        }

        class DisposableController : IDisposable
        {
            public static bool Invoked { get; private set; }

            public static bool Disposed { get; private set; }

            public void Invoke()
            {
                Invoked = true;
            }

            public void Dispose()
            {
                Disposed = true;
            }
        }
    }
}
