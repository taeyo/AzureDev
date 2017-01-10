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
            string token = string.Empty;

            string azureMediaAccount = CloudConfigurationManager.GetSetting("AzureMediaAccount");
            string azureMediaAccessKey = CloudConfigurationManager.GetSetting("AzureMediaAccessKey");

            CloudMediaContext _mediaContext = new CloudMediaContext(azureMediaAccount, azureMediaAccessKey);

            Console.WriteLine("Listing Assets.....");
            Console.WriteLine("#######################################################");

            foreach (var asset in _mediaContext.Assets)
            {
                Console.WriteLine($"Asset Name : {asset.Name}");
                Console.WriteLine($"Asset ID : {asset.Id}");

                if (asset.ContentKeys.Count > 0) Console.WriteLine("Listing ContentKeys.....");

                // ContentKeys 추출 및 나열
                foreach (var contentKey in asset.ContentKeys)
                {
                    Console.WriteLine($"    ContentKeys Name : {contentKey.Name}");
                    Console.WriteLine($"    ContentKeys id : {contentKey.Id}");

                    // AuthorizationPolicy 추출 및 나열
                    if (contentKey.AuthorizationPolicyId != null)
                    {
                        IContentKeyAuthorizationPolicy myAuthPolicy = _mediaContext.ContentKeyAuthorizationPolicies.Where(p => p.Id == contentKey.AuthorizationPolicyId).FirstOrDefault();
                        if (myAuthPolicy != null && myAuthPolicy.Options.Count > 0)
                        {
                            // 허가정책이 없으면 토큰을 생성할 수 없음
                            Console.WriteLine($"       [List of AuthorizationPolicyOptions]");

                            // 각 허가정책마다 토큰을 생성
                            foreach (var option in myAuthPolicy.Options)
                            {
                                Console.WriteLine($"        Name : {option.Name}");
                                Console.WriteLine($"        AuthorizationPolicyOption : {option.Id}");

                                // 실제 토큰 생성
                                token = GetTokenString(_mediaContext, contentKey, option);

                                Console.WriteLine($"        Token : {token}");
                                Console.WriteLine("-----------------------------------------------------");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(" -- Can't create token because AuthorizationPolicyId is not exist");
                    }
                    Console.WriteLine("----------------------- End of ContentKey -----------------------");
                }
                Console.WriteLine("####################### End of Asset ##########################");
                Console.WriteLine("");
            }

            Console.ReadLine();
        }

        private static string GetTokenString(CloudMediaContext _mediaContext, IContentKey contentKey, IContentKeyAuthorizationPolicyOption SelectedOption, DateTime? tokenExpiration = null)
        {
            DateTime dateTokenExpiration = tokenExpiration ?? DateTime.Now.AddHours(1);

            // ContentKeyAuthorizationPolicyOption을 선택하고, 그로부터 토큰 템플릿을 추출
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

                // Symmetric인 경우
                if (tokenTemplate.PrimaryVerificationKey.GetType() == typeof(SymmetricVerificationKey))
                {
                    InMemorySymmetricSecurityKey tokenSigningKey = new InMemorySymmetricSecurityKey((tokenTemplate.PrimaryVerificationKey as SymmetricVerificationKey).KeyValue);
                    signingcredentials = new SigningCredentials(tokenSigningKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
                }
                // X509인 경우
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

            return $"Bearer {testToken}";
        }
    }
}

