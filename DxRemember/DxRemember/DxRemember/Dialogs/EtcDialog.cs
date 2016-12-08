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

            content = content.Replace('\"', ' ');

            string name, phone, email;

            string namePattern = @"[가-힣| *]{2,4}|[a-zA-Z]{2,10}\s[a-zA-Z]{2,10}";
            string emailPattern = @"([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)";

            Match match = Regex.Match(content, namePattern);
            if (match.Success)
            {
                name = match.Captures[0].Value;
            }

            match = Regex.Match(content, emailPattern);
            if (match.Success)
            {
                email = match.Captures[0].Value;
            }

            int iPos = content.IndexOf("text : 010");
            iPos = content.IndexOf("text", iPos + 2);
            string fnum = content.Substring(iPos + 7, 4);
            iPos = content.IndexOf("text", iPos + 2);
            string lnum = content.Substring(iPos + 7, 4);



            context.Done(0);

            context.Wait(this.MessageReceivedAsync);
        }

    }
}