using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Console_OCRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            OCRClient ocr = new OCRClient("*************", "https://api.projectoxford.ai/vision/v1.0/ocr");
            Task<HttpResponseMessage> msg = ocr.EvaluateImageAsync();

            Task<string> result = ocr.ProcessResponseAsync(msg.Result);

            Console.WriteLine(result.Result);
            Console.ReadLine();
        }
    }
}
