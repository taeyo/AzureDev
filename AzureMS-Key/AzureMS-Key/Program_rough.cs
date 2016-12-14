using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.MediaServices.Client.ContentKeyAuthorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AzureMS_Key
{
    class Program_rough
    {
        
        static void Main(string[] args)
        {
            string tokenTemplateString = string.Empty;
            TokenRestrictionTemplate tokenTemplate = null;
            IContentKeyAuthorizationPolicyOption selectedOption = null;
            Guid rawkey;
            string token = string.Empty;
            string name = string.Empty;
            string id = string.Empty;

            CloudMediaContext _mediaContext = new CloudMediaContext(@"taeyoams", @"JikvgXjlQIkFlxZe+P/TVhAaJ08msLao6OarD+gpS24=");

            Console.WriteLine("Listing Assets");
            foreach (var asset in _mediaContext.Assets)
            {
                Console.WriteLine("Asset Name : {0}", asset.Name);
                Console.WriteLine("Asset ID : {0}", asset.Id);

                if (asset.ContentKeys.Count > 0) Console.WriteLine("Listing ContentKeys");
                foreach (var contentKey in asset.ContentKeys)
                {
                    Console.WriteLine("    ContentKeys Name : {0}", contentKey.Name);
                    Console.WriteLine("    ContentKeys id : {0}", contentKey.Id);

                    var options = ListContentKeyAuthorizationPolicyOptions(_mediaContext, contentKey);
                    selectedOption = options.FirstOrDefault();

                    //첫번째 Option을 무조건 채택했음.
                    tokenTemplateString = selectedOption.Restrictions.FirstOrDefault().Requirements;
                    tokenTemplate = TokenRestrictionTemplateSerializer.Deserialize(tokenTemplateString);

                    // 콘텐트 키를 GUID 형식으로 바꿔줌.
                    rawkey = EncryptionUtils.GetKeyIdAsGuid(contentKey.Id);
                    token = GetTokenString(_mediaContext, contentKey, selectedOption);

                    Console.WriteLine("Token : " + token);
                    Console.WriteLine("------------------------------------------------");
                }
                Console.WriteLine("#######################################################");
            }

            //특정 ContentKey 찾기
            //string strContentKey = "ContentKey Envelope";
            //IContentKey contentKey = _mediaContext.ContentKeys.Where(c => c.Name == strContentKey).First();
            //Uri keyDeliveryServiceUri = contentKey.GetKeyDeliveryUrl(ContentKeyDeliveryType.BaselineHttp);

            //첫번째 Option을 무조건 채택했음.
            // ContentKeyAuthorizationPolicyOption을 선택하고, 그로부터 토큰 템플릿을 추출
            //IContentKeyAuthorizationPolicyOption selectedOption = null;
            //IContentKeyAuthorizationPolicy myAuthPolicy = _mediaContext.ContentKeyAuthorizationPolicies.Where(p => p.Id == contentKey.AuthorizationPolicyId).FirstOrDefault();
            //if (myAuthPolicy != null)
            //{
            //    selectedOption = myAuthPolicy.Options[0];
            //}

            //string tokenTemplateString = selectedOption.Restrictions.FirstOrDefault().Requirements;
            //TokenRestrictionTemplate tokenTemplate = TokenRestrictionTemplateSerializer.Deserialize(tokenTemplateString);

            //// 콘텐트 키를 GUID 형식으로 바꿔줌.
            //Guid rawkey = EncryptionUtils.GetKeyIdAsGuid(contentKey.Id);
            //string token = GetTokenString(_mediaContext, contentKey, selectedOption);

            //Console.WriteLine(token);
            Console.ReadLine();
        }

        static void Main_Back(string[] args)
        {
            CloudMediaContext _mediaContext = new CloudMediaContext(@"taeyoams", @"JikvgXjlQIkFlxZe+P/TVhAaJ08msLao6OarD+gpS24=");

            Console.WriteLine("Listing ContentKeys");
            foreach (var key in _mediaContext.ContentKeys)
            {
                Console.WriteLine("Name : {0}, Kid : {1}", key.Name, key.Id);
            }

            //Console.WriteLine("Listing ContentKeyAuthorizationPolicyOptions");
            // IContentKeyAuthorizationPolicyOption 목록 나열하기.
            //IContentKeyAuthorizationPolicyOption SelectedOption = _mediaContext.ContentKeyAuthorizationPolicyOptions.Where(p => p.Id == "").FirstOrDefault();
            //foreach (var option in _mediaContext.ContentKeyAuthorizationPolicyOptions)
            //{
            //    string keyName = option.Name;
            //    string keyId = option.Id;

            //    Console.WriteLine("Name : {0}, AuthorizationPolicyOption : {1}", keyName, keyId);
            //}



            //특정 ContentKey 찾기
            string strContentKey = "ContentKey Envelope";
            IContentKey contentKey = _mediaContext.ContentKeys.Where(c => c.Name == strContentKey).First();
            //Uri keyDeliveryServiceUri = contentKey.GetKeyDeliveryUrl(ContentKeyDeliveryType.BaselineHttp);

            IContentKeyAuthorizationPolicyOption selectedOption = null;
            IContentKeyAuthorizationPolicy myAuthPolicy = _mediaContext.ContentKeyAuthorizationPolicies.Where(p => p.Id == contentKey.AuthorizationPolicyId).FirstOrDefault();
            if (myAuthPolicy != null)
            {
                foreach (var option in myAuthPolicy.Options)
                {
                    selectedOption = option;
                    Console.WriteLine("Name : {0}, AuthorizationPolicyOption : {1}", option.Name, option.Id);
                }
            }

            // ContentKeyAuthorizationPolicyOption을 선택하고, 그로부터 토큰 템플릿을 추출
            //IContentKeyAuthorizationPolicyOption SelectedOption = _mediaContext.ContentKeyAuthorizationPolicyOptions.FirstOrDefault();
            string tokenTemplateString = selectedOption.Restrictions.FirstOrDefault().Requirements;
            TokenRestrictionTemplate tokenTemplate = TokenRestrictionTemplateSerializer.Deserialize(tokenTemplateString);

            //contentKey.

            //Name: ContentKey Envelope, Kid : nb: kid: UUID: 3e1adc8c-bc4a-4541-b3a0-dc4b0ec6740b
            //Name: ContentKey Envelope, Kid : nb: kid: UUID: 615fa67f-55f7-456b-bd95-f6add6f8c191

            // 콘텐트 키를 GUID 형식으로 바꿔줌.
            Guid rawkey = EncryptionUtils.GetKeyIdAsGuid(contentKey.Id);

            string token = GetTokenString(_mediaContext, contentKey, selectedOption);

            //string swtToken = GenerateTokenRequirements();
            //TokenRestrictionTemplate tokenTemplate = TokenRestrictionTemplateSerializer.Deserialize(swtToken);
            //TokenRestrictionTemplate tokenTemplate = TokenRestrictionTemplateSerializer.Deserialize(tokenTemplateString);

            //string textSwtToken = TokenRestrictionTemplateSerializer.GenerateTestToken(tokenTemplate);

            //The GenerateTestToken method returns the token without the word “Bearer” in front
            //so you have to add it in front of the token string. 

            //string testToken = TokenRestrictionTemplateSerializer.GenerateTestToken(tokenTemplate, null, rawkey);
            //string swtTokenString = TokenRestrictionTemplateSerializer.GenerateTestToken(tokenTemplate,
            //tokenTemplate.PrimaryVerificationKey, rawkey, DateTime.Now.AddDays(2));

            Console.WriteLine(token);
            Console.ReadLine();

            //string uriWithTokenParameter = string.Format("{0}&token={1}", keyDeliveryServiceUri.AbsoluteUri, swtToken);
            //Uri keyDeliveryUrlWithTokenParameter = new Uri(uriWithTokenParameter);

            //byte[] bs = GetDeliveryKey(keyDeliveryServiceUri, swtToken);

            ////Adding Auth header to content key request 
            //var kdClient = new HttpClient();
            ////kdClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtTokenString);
            //kdClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", testToken);

            ////Sending Post request
            //HttpResponseMessage response = kdClient.PostAsync(keyDeliveryServiceUri, new StringContent(string.Empty)).Result;
            //response.EnsureSuccessStatusCode();
            ////Reading HLS content key
            //byte[] hlsContentKey = response.Content.ReadAsStreamAsync().ContinueWith(t =>
            //{
            //    Stream stream = t.Result;
            //    var bytes = new byte[stream.Length];
            //    stream.Read(bytes, 0, (int)stream.Length);
            //    return bytes;
            //}).Result;

            //string s = hlsContentKey != null ? System.Text.Encoding.UTF8.GetString(hlsContentKey) : null;

            //Console.WriteLine(s);
        }

        private static IList<IContentKeyAuthorizationPolicyOption> ListContentKeyAuthorizationPolicyOptions(CloudMediaContext _mediaContext, IContentKey contentKey)
        {
            IContentKeyAuthorizationPolicy myAuthPolicy = _mediaContext.ContentKeyAuthorizationPolicies.Where(p => p.Id == contentKey.AuthorizationPolicyId).FirstOrDefault();
            if (myAuthPolicy != null)
            {
                Console.WriteLine("        AuthorizationPolicyOptions List", contentKey.Name);
                foreach (var option in myAuthPolicy.Options)
                {
                    string name = option.Name;
                    string id = option.Id;

                    Console.WriteLine("        Name : {0}", name);
                    Console.WriteLine("        AuthorizationPolicyOption : {0}", id);
                }

                return myAuthPolicy.Options;
            }

            return null;
        }

        private static string GetTokenString(CloudMediaContext _mediaContext, IContentKey contentKey, IContentKeyAuthorizationPolicyOption SelectedOption, DateTime? tokenExpiration = null)
        {
            DateTime dateTokenExpiration = tokenExpiration ?? DateTime.Now.AddHours(1);

            // ContentKeyAuthorizationPolicyOption을 선택하고, 그로부터 토큰 템플릿을 추출
            //IContentKeyAuthorizationPolicyOption SelectedOption = _mediaContext.ContentKeyAuthorizationPolicyOptions.FirstOrDefault();
            string tokenTemplateString = SelectedOption.Restrictions.FirstOrDefault().Requirements;
            TokenRestrictionTemplate tokenTemplate = TokenRestrictionTemplateSerializer.Deserialize(tokenTemplateString);

            // 콘텐트 키를 GUID 형식으로 바꿔줌.
            Guid rawkey = EncryptionUtils.GetKeyIdAsGuid(contentKey.Id);

            string testToken = string.Empty;
            SigningCredentials signingcredentials = null;

            //SWT
            if (tokenTemplate.TokenType == TokenType.SWT) 
            {
                testToken = TokenRestrictionTemplateSerializer.GenerateTestToken(tokenTemplate, null, rawkey, dateTokenExpiration);
            }
            else // JWT
            {
                // 클레임이 있으면 그 부분을 따로 목록화해서 JWT에 추가함.
                IList<Claim> myclaims = new List<Claim>();
                foreach (var claim in tokenTemplate.RequiredClaims)
                {
                    if (claim.ClaimType == TokenClaim.ContentKeyIdentifierClaimType)
                    {
                        myclaims.Add(new Claim(TokenClaim.ContentKeyIdentifierClaimType, rawkey.ToString()));
                    }
                    else
                    {
                        myclaims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
                    }
                }

                // SWT Symmetric인 경우
                if (tokenTemplate.PrimaryVerificationKey.GetType() == typeof(SymmetricVerificationKey))
                {
                    InMemorySymmetricSecurityKey tokenSigningKey = new InMemorySymmetricSecurityKey((tokenTemplate.PrimaryVerificationKey as SymmetricVerificationKey).KeyValue);
                    signingcredentials = new SigningCredentials(tokenSigningKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
                }
                // SWT X509인 경우
                else if (tokenTemplate.PrimaryVerificationKey.GetType() == typeof(X509CertTokenVerificationKey))
                {
                    throw new NotImplementedException("아직 구현하지 않음");

                    //X509Certificate2 cert = form.GetX509Certificate;
                    //if (cert != null) signingcredentials = new X509SigningCredentials(cert);
                }

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: tokenTemplate.Issuer.ToString(), 
                    audience: tokenTemplate.Audience.ToString(), 
                    notBefore: DateTime.Now.AddMinutes(-10), 
                    expires: dateTokenExpiration, 
                    signingCredentials: signingcredentials, 
                    claims: myclaims);
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                testToken = handler.WriteToken(token);
            }            

            return "Bearer " + testToken; ;
        }


        private static byte[] GetDeliveryKey(Uri keyDeliveryUri, string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(keyDeliveryUri);

            request.Method = "POST";
            request.ContentType = "text/xml";
            if (!string.IsNullOrEmpty(token))
            {
                //: Bearer urn% 3amicrosoft % 3aazure % 3amediaservices % 3acontentkeyidentifier = &Audience = urn % 3atest & ExpiresOn = 1481515588 & Issuer = http % 3a % 2f % 2ftestacs & HMACSHA256 = zpJ7C4YkCGKmHctqmyuaC9hQfOkgHLhXL2aRS0eFS3Y % 3d
                request.Headers["Authorization"] = "Bearer=" + token;
            }
            request.ContentLength = 0;

            var response = request.GetResponse();

            var stream = response.GetResponseStream();
            if (stream == null)
            {
                throw new NullReferenceException("Response stream is null");
            }

            var buffer = new byte[256];
            var length = 0;
            while (stream.CanRead && length <= buffer.Length)
            {
                var nexByte = stream.ReadByte();
                if (nexByte == -1)
                {
                    break;
                }
                buffer[length] = (byte)nexByte;
                length++;
            }
            response.Close();

            // AES keys must be exactly 16 bytes (128 bits).
            var key = new byte[length];
            Array.Copy(buffer, key, length);
            return key;
        }


        static private string GenerateTokenRequirements()
        {
            string issuer = "http://testacs";
            string audience = "urn:test";

            TokenRestrictionTemplate template = new TokenRestrictionTemplate(TokenType.SWT);

            template.PrimaryVerificationKey = new SymmetricVerificationKey();
            template.AlternateVerificationKeys.Add(new SymmetricVerificationKey());
            template.Audience = audience;
            template.Issuer = issuer;

            template.RequiredClaims.Add(TokenClaim.ContentKeyIdentifierClaim);

            return TokenRestrictionTemplateSerializer.Serialize(template);
        }

        public static string GenerateTestToken(TokenRestrictionTemplate tokenTemplate, TokenVerificationKey signingKeyToUse = null, Guid? keyIdForContentKeyIdentifierClaim = null, DateTime? tokenExpiration = null)
        {
            if (tokenTemplate == null)
            {
                throw new ArgumentNullException("tokenTemplate");
            }

            if (signingKeyToUse == null)
            {
                //3r5xCRgLKKOtiBin5uCUPpUeMdPbGMKINLXAWaav6TbZzjLZM6AHcwU73YZw5c1MtOaVYcsIrUQ5CqLWR3RQpw==
                signingKeyToUse = tokenTemplate.PrimaryVerificationKey;

                byte[] primaryVerificationKey = ((SymmetricVerificationKey)signingKeyToUse).KeyValue;

                string s3 = Convert.ToBase64String(primaryVerificationKey);  // gsjqFw==

                string s4 = HttpServerUtility.UrlTokenEncode(primaryVerificationKey);

                //signingKeyToUse = tokenTemplate.AlternateVerificationKeys;

                //byte[] alternateVerificationKeys = ((SymmetricVerificationKey)signingKeyToUse).KeyValue;

                //string s5 = Convert.ToBase64String(alternateVerificationKeys);  // gsjqFw==

                //string s6 = HttpServerUtility.UrlTokenEncode(alternateVerificationKeys);


                int i = 0;
            }

            if (!tokenExpiration.HasValue)
            {
                tokenExpiration = DateTime.UtcNow.AddMinutes(60);
            }

            StringBuilder builder = new StringBuilder();

            foreach (TokenClaim claim in tokenTemplate.RequiredClaims)
            {
                string claimValue = claim.ClaimValue;
                if (claim.ClaimType == TokenClaim.ContentKeyIdentifierClaimType)
                {
                    claimValue = keyIdForContentKeyIdentifierClaim.ToString();
                }

                builder.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(claim.ClaimType), HttpUtility.UrlEncode(claimValue));
            }

            builder.AppendFormat("Audience={0}&", HttpUtility.UrlEncode(tokenTemplate.Audience));
            builder.AppendFormat("ExpiresOn={0}&", GenerateTokenExpiry(tokenExpiration.Value));/*GenerateTokenExpiry(tokenExpiration.Value)*/
            builder.AppendFormat("Issuer={0}", HttpUtility.UrlEncode(tokenTemplate.Issuer));

            SymmetricVerificationKey signingKey = (SymmetricVerificationKey)signingKeyToUse;
            using (var signatureAlgorithm = new HMACSHA256(signingKey.KeyValue))
            {
                byte[] unsignedTokenAsBytes = Encoding.UTF8.GetBytes(builder.ToString());

                byte[] signatureBytes = signatureAlgorithm.ComputeHash(unsignedTokenAsBytes);

                string signatureString = Convert.ToBase64String(signatureBytes);

                builder.AppendFormat("&HMACSHA256={0}", HttpUtility.UrlEncode(signatureString));
            }

            return builder.ToString();
        }

        private static readonly DateTime SwtBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static string GenerateTokenExpiry(DateTime expiry)
        {
            return ((long)expiry.Subtract(SwtBaseTime).TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }
    }
}

