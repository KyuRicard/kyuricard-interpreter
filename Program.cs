using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{
    class Program
    {
        static void Main(string[] args)
        {
            Interpreter.Init();
            string line = "";
            while (true)
            {
                Console.Write(">");
                line = Console.ReadLine();
                string[] lines = line.Split(' ');
                Interpreter.Interprete(lines);
            }
        }
    }
}
