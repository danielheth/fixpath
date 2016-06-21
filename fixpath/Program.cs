using System;
using System.Collections.Generic;
using System.Text;

namespace cygfixpath
{
    class Program
    {
        static void Main(string[] args)
        {
            var arg = string.Join(" ", args);
            Console.Write(arg.Replace("/","\\"));
        }
    }
}
