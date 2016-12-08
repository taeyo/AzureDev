using Microsoft.Azure;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DxRemember.Utils
{
    public class OcrHelper
    {
        public async Task<string> Process(IDialogContext context, string fileUri)
        {
            string subscriptKey = CloudConfigurationManager.GetSetting("Ocp-Apim-Subscription-Key");
            string ocrApiRoot = CloudConfigurationManager.GetSetting("Ocr-Api-Root");
            string text = string.Empty;

            Utils.OCRClient ocr = new Utils.OCRClient(subscriptKey, ocrApiRoot);

            //Retry 
            var exceptions = new List<Exception>();
            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        if (retry == 1)
                            await context.PostAsync("인식 서비스를 통해서 텍스트를 추출하고 있습니다. 잠시만 기다려 주세요...");
                        else
                            await context.PostAsync("작업이 지연되고 있습니다. 잠시만 기다려 주십시오....");

                        Task<HttpResponseMessage> msg = ocr.EvaluateImageAsync(fileUri);

                        Task<string> textResult = ocr.ProcessResponseAsync(msg.Result);
                        text = textResult.Result;

                        // Return or break.
                        break;
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 1)
            {
                throw new AggregateException(exceptions);
            }

            //await context.PostAsync(text);
            return text;
        }
    }
}