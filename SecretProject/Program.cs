using IronOcr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var ocr = new ImageToText(@"C:\work\test.png");
            var res = ocr.ReadText(500,850,900,100);
            Console.WriteLine(res);

        }
    }
}
