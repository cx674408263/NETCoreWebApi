using Microsoft.IdentityModel.Tokens;
using NETCore.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NETCore.Components.Helper
{
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModel tokenModel)
        {
            //获取Appsetting配置
            string iss = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "Issuer" });
            string aud = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "Audience" });
            string secret = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "SecretKey" });

            //var claims = new Claim[] //old
            var claims = new List<Claim>
                {
                 /*
                 * 特别重要：
                   1、这里将用户的部分信息，比如 uid 存到了Claim 中，如果你想知道如何在其他地方将这个 uid从 Token 中取出来，请看下边的SerializeJwt() 方法，或者在整个解决方案，搜索这个方法，看哪里使用了！
                   2、你也可以研究下 HttpContext.User.Claims ，具体的你可以看看 Policys/PermissionHandler.cs 类中是如何使用的。
                 */



                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //这个就是过期时间，目前是过期1000秒，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(1000).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud),


               };

            // 可以将一个用户的多个角色全部赋予；
            claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));



            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModel
            {
                Uid = jwtToken.Id.ToString(),
                Role = role != null ? role.ToString() : "",
            };
            return tm;
        }
    }
}
/*
 
  常见疑惑解析
1、JWT里会存在一些用户的信息，比如用户id、角色role 等等，这样会不会不安全，信息被泄露？
      答：JWT 本来就是一种无状态的登录授权认证，用来替代每次请求都需要输入用户名+密码的尴尬情况，存在一些不重要的明文很正常，只要不把隐私放出去就行，就算是被动机不良的人得到，也做不了什么事情。


2、生成 JWT 的时候需要 secret ，但是 解密的时候 为啥没有用到 secret ？
答：secret的作用，主要是用来防止 token 被伪造和篡改的，想想上边的那个第一个问题，
用户得到了你的令牌，获取到了你的个人信息，这个是没事儿的，他什么也干不了，但是如果用户自己随便的生成一个 token ，
带上你的uid，岂不是随便就可以访问资源服务器了，所以这个时候就需要一个 secret 来生成 token，这样的话，就能保证数字签名的正确性。
 
 
 
 
 
 
 
 
 
 */