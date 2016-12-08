namespace DxRemember
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Azure;
    using System.Net.Http;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

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
            string fileUri = "https://studydemo.blob.core.windows.net/bizcards/IMG_1328.JPG";
            Utils.OcrHelper ocr = new Utils.OcrHelper();
            string content = await ocr.Process(context, fileUri);

            Utils.RegUtils reg = new Utils.RegUtils();
            List<string> msgs = reg.ExtractAndFormatData(content);

            await context.PostAsync("추출된 고객의 정보입니다");

            foreach (string s in msgs)
            {
                await context.PostAsync(s);
            }

            context.Done(0);

            context.Wait(this.MessageReceivedAsync);
        }

    }
}