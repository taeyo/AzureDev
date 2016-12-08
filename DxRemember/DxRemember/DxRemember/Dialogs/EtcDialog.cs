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
            string fileUri = "https://studydemo.blob.core.windows.net/bizcards/IMG_1328.JPG";
            Utils.OcrHelper ocr = new Utils.OcrHelper();
            ocr.Process(context, fileUri);
            context.Done(0);

            context.Wait(this.MessageReceivedAsync);
        }

    }
}