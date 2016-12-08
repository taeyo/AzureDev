namespace DxRemember
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Azure;
    using System.Net.Http;
    using System.Collections.Generic;

    [Serializable]
    public class EtcDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {

            //context.Fail(new NotImplementedException("아직 기능이 구현되지 않았습니다."));

            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string subscriptKey = CloudConfigurationManager.GetSetting("Ocp-Apim-Subscription-Key");
            string ocrApiRoot = CloudConfigurationManager.GetSetting("Ocr-Api-Root");
            string text = string.Empty;

            string fileUri = "https://studydemo.blob.core.windows.net/bizcards/IMG_1328.JPG";
            Utils.OCRClient ocr = new Utils.OCRClient(subscriptKey, ocrApiRoot);

            //Retry 
            var exceptions = new List<Exception>();
            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
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

            if(exceptions.Count > 1)
            {
                throw new AggregateException(exceptions);
            }

            await context.PostAsync(text);

            context.Wait(this.MessageReceivedAsync);
        }

    }
}