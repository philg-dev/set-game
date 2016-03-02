using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //int i = 5;
            //Console.WriteLine("{0:00}", i);
            //Console.ReadLine();

            int[] k = new int[10];
            int[] j = new int[10];
            int[] m = new int[10];
            for (int i = 0; i < 10; i++)
            {
                k[i] = i;
                j[i] = 2 * i;
                m[i] = k[i] + j[i];
                Console.WriteLine("{0,2:D} + {1,2:D} = {2,2:D}", k[i], j[i], m[i]);
                /*
                0 +  0 =  0
                1 +  2 =  3
                2 +  4 =  6
                3 +  6 =  9
                4 +  8 = 12
                5 + 10 = 15
                6 + 12 = 18
                7 + 14 = 21
                8 + 16 = 24
                9 + 18 = 27
                */
            }
            Console.ReadLine();
        }
    }
}
