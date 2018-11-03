using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            int x = Factorial(10);


            double[] sam = { 4.2, 4.1, 7, 2, 0 };

            Array.Sort(sam);

            foreach (double value in sam)
            {
                Console.Write(value);
                Console.Write(' ');
            }

            Console.ReadKey();
        }
        static int Factorial(int x)
        {
            if (x == 0)
            {
                return 1;
            }
            else
            {
                return x * Factorial(x - 1);
            }
        }
    }
}
