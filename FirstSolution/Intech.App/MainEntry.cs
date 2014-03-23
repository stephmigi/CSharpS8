using Intech.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.App
{
    class MainEntry
    {
        static void Main(string[] args)
        {
            string rootPath = "C:\\Temp";

            FileProcessor fileProcessor = new FileProcessor();
            var result = fileProcessor.Process(rootPath);
            result.RenderResultsInConsole();

            Console.ReadLine();
        }
    }
}
