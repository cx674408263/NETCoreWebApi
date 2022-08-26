using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Components
{

    /*为什么使用Swagger
    随着互联网技术的发展，现在的网站架构基本都由原来的后端渲染，变成了：前端渲染、后端分离的形态，而且前端技术和后端技术在各自的道路上越走越远。 

    前端和后端的唯一联系，变成了API接口；API文档变成了前后端开发人员联系的纽带，变得越来越重要，swagger就是一款让你更好的书写API文档的框架。

    没有API文档工具之前，大家都是手写API文档的，在什么地方书写的都有，有在confluence上写的，有在对应的项目目录下readme.md上写的，
    每个公司都有每个公司的玩法，无所谓好坏。

    书写API文档的工具有很多，但是能称之为“框架”的，估计也只有swagger了。
     */
    public static class SwaggerSetUp
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var ApiName = "NETCore";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    // {ApiName} 定义成全局变量，方便修改
                    Version = "V1",
                    Title = $"{ApiName} 接口文档  NETCore 3.1 v1版本",
                    Description = $"{ApiName} HTTP API V1",

                });
                c.OrderActionsBy(o => o.RelativePath);

                // 获取xml注释文件的目录(Api)
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "NETCoreWebApi.xml");
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                // 获取xml注释文件的目录(Model)
                var xmlModelPath = Path.Combine(AppContext.BaseDirectory, "NETCore.Model.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);


                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                #region Token绑定到ConfigureServices
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });

        }
    }
}
