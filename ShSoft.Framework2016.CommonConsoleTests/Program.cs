using System;
using ShSoft.Framework2016.Common.PoweredByLee;

namespace ShSoft.Framework2016.CommonConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            人员[] excels = ExcelAssistant.ReadFile<人员>("StubExcel.xls", "default", 1);

            Console.WriteLine(excels.Length);
            Console.ReadKey();
        }
    }
}
