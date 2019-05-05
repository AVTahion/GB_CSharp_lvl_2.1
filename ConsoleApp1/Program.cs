using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numer = { 4, 8, 16, 32, 64, 128 };
            int[] denom = { 2, 0, 4, 4, 0, 8 };
            for (int i = 0; i < numer.Length; i++)
            {
                
                    Console.WriteLine(numer[i] + " / " +
                                      denom[i] + " is " +
                                      numer[i] / denom[i]);
                
            }
            Console.ReadKey();
        }

    }
}
