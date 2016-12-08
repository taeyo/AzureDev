using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Console_OCRClient
{
    class OCRClient// : ClientBase
    {
        protected string SubscriptionKey;

        protected string ApiRoot = "https://api.projectoxford.ai/vision/v1.0/ocr";

        protected int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout

        string method = "GET";

        public OCRClient(string subscriptionKey) : this(subscriptionKey, null) { }

        public OCRClient(string subscriptionKey, string apiRoot)
        {
            this.ApiRoot = apiRoot?.TrimEnd('/');
            this.SubscriptionKey = subscriptionKey;
        }

        public async Task<HttpResponseMessage> EvaluateImageAsync()
        {

            List<KeyValue> metaData = new List<KeyValue>();
            metaData.Add(new KeyValue()
            {
                Key = "language",
                Value = "unk"
            });
            metaData.Add(new KeyValue()
            {
                Key = "detectOrientation",
                Value = "true"
            });

            //this.InvokeAsync<string>(),
            StringBuilder requestUrl = new StringBuilder(string.Concat(this.ApiRoot, "?"));

            if (metaData != null)
            {
                foreach (var k in metaData)
                {
                    requestUrl.Append(string.Concat(k.Key, "=", k.Value));
                    requestUrl.Append("&");
                }
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string json = "{\"url\":\"https://studydemo.blob.core.windows.net/bizcards/IMG_1328.JPG\"}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync(requestUrl.ToString(), content).ConfigureAwait(false);
        }

        public async Task<string> ProcessResponseAsync(HttpResponseMessage webResponse)
        {
            using (webResponse)
            {
                if (webResponse.StatusCode == HttpStatusCode.OK ||
                    webResponse.StatusCode == HttpStatusCode.Accepted ||
                    webResponse.StatusCode == HttpStatusCode.Created)
                {
                    if (webResponse.Content.Headers.ContentLength != 0)
                    {
                        return await webResponse.Content.ReadAsStringAsync();
                    }
                }
            }

            return string.Empty;
        }
    }
}
