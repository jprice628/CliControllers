using CliControllers;
using System;

namespace Calculator
{
    // Try the following from the command line:
    //     /> calc add 123 45
    //     /> calc + 355 13
    [Description("Adds two integers together.")]
    [Alias("+")]
    public class AddController
    {
        public void Invoke(
            [Argument]
            [Description("An integer.")]
            int a,

            [Argument]
            [Description("An integer.")]
            int b
            )
        {
            Console.WriteLine($"{a} + {b} = {a + b}");
        }
    }
}
