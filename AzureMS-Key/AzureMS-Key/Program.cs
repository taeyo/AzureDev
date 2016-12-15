using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.MediaServices.Client.ContentKeyAuthorization;
using Microsoft.WindowsAzure;

namespace AzureMS_Key
{
    class Program
    {
        /// <summary>
        /// Azure Media Service에서 Token 기반의 DRM이 걸려있는 Asset에 대해서 자동으로 SWT, JWT 토큰을 만드는 예제
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string tokenTemplateString = string.Empty;
            TokenRestrictionTemplate tokenTemplate = null;
            IContentKeyAuthorizationPolicyOption selectedOption = null;
            Guid rawkey;
            string token = string.Empty;

            string azureMediaAccount = CloudConfigurationManager.GetSetting("AzureMediaAccount");
            string azureMediaAccessKey = CloudConfigurationManager.GetSetting("AzureMediaAccessKey");

            CloudMediaContext _mediaContext = new CloudMediaContext(azureMediaAccount, azureMediaAccessKey);

            Console.WriteLine("Listing Assets");
            foreach (var asset in _mediaContext.Assets)
            {
                Console.WriteLine($"Asset Name : {asset.Name}");
                Console.WriteLine($"Asset ID : {asset.Id}");

                if (asset.ContentKeys.Count > 0) Console.WriteLine("Listing ContentKeys");
                foreach (var contentKey in asset.ContentKeys)
                {
                    Console.WriteLine($"    ContentKeys Name : {contentKey.Name}");
                    Console.WriteLine($"    ContentKeys id : {contentKey.Id}");

                    var options = ListContentKeyAuthorizationPolicyOptions(_mediaContext, contentKey);
                    if(options == null)
                    {
                        Console.WriteLine("Can not create Token because AuthorizationPolicyId is not exist");
                        continue;
                    }

                    selectedOption = options.FirstOrDefault();

                    //첫번째 Option을 무조건 채택했음.
                    tokenTemplateString = selectedOption.Restrictions.FirstOrDefault().Requirements;
                    tokenTemplate = TokenRestrictionTemplateSerializer.Deserialize(tokenTemplateString);

                    // 콘텐트 키를 GUID 형식으로 바꿔줌.
                    rawkey = EncryptionUtils.GetKeyIdAsGuid(contentKey.Id);
                    token = GetTokenString(_mediaContext, contentKey, selectedOption);

                    Console.WriteLine("Token : " + token);
                    Console.WriteLine("");
                }
                Console.WriteLine("#######################################################");
                Console.WriteLine("");
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

        private static IList<IContentKeyAuthorizationPolicyOption> ListContentKeyAuthorizationPolicyOptions(CloudMediaContext _mediaContext, IContentKey contentKey)
        {
            IList<IContentKeyAuthorizationPolicyOption> options = null;

            if (contentKey.AuthorizationPolicyId != null)
            {
                IContentKeyAuthorizationPolicy myAuthPolicy = _mediaContext.ContentKeyAuthorizationPolicies.Where(p => p.Id == contentKey.AuthorizationPolicyId).FirstOrDefault();
                if (myAuthPolicy != null)
                {
                    Console.WriteLine($"        AuthorizationPolicyOptions List");
                    foreach (var option in myAuthPolicy.Options)
                    {
                        Console.WriteLine($"        Name : {option.Name}");
                        Console.WriteLine($"        AuthorizationPolicyOption : {option.Id}");
                    }

                    options = myAuthPolicy.Options;
                }
            }

            return options;
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
                    notBefore: null, 
                    expires: dateTokenExpiration, 
                    signingCredentials: signingcredentials, 
                    claims: myclaims);
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                testToken = handler.WriteToken(token);
            }            

            return "Bearer " + testToken; ;
        }
    }
}

