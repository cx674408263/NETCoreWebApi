﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NETCore.Components.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Components.Setup
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 1【授权】、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Role", policy => policy.RequireRole("Role").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));

            });


            //读取配置文件
            var symmetricKeyAsBase64 = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "SecretKey" });
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "Issuer" });
            var Audience = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "Audience" });



            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,//发行人
                ValidateAudience = true,
                ValidAudience = Audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),
                RequireExpirationTime = true,
            };

            //2.1【认证】、core自带官方JWT认证
            // 开启Bearer认证
            services.AddAuthentication("Bearer")
             // 添加JwtBearer服务
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         // 如果过期，则把<是否过期>添加到，返回头信息中
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             });



        }
    }
}
