using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliControllers;

namespace Calculator
{
    // Try the following from the command line:
    //     /> calc divide 121 11
    //     /> calc / 10 3
    //     /> calc div 10 3 -r
    [Description("Divides one integer values by another.")]
    [Alias("/ div")]
    public class DivideController
    {
        public void Invoke(
            [Argument]
            [Description("The number to be divided into groups.")]
            int dividend,

            [Argument]
            [Description("The number of groups into which to divide the dividend.")]
            int divisor,

            [Option("false")]
            [Description("When specified, shows the remainder of the operation.")]
            [Alias("r")]
            bool showRemainder
            )
        {
            if (divisor == 0) throw new ArgumentException("Divisor cannot be zero.");

            Console.Write($"{dividend} / {divisor} = {dividend / divisor}");

            if (showRemainder)
            {
                Console.Write($"r{dividend % divisor}");
            }

            Console.WriteLine();
        }
    }
}
