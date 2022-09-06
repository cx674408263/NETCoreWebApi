
using Autofac;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.Caching.Redis;
using NETCore.Components;
using NETCore.Components.Helper;
using NETCore.Components.Setup;
using NETCore.Cores;
using NETCore.Filter;
using NETCore.Repository.Sugar;
using System;
using NETCore.Middlewares;
using NETCore.Loging.Log4;

namespace NETCoreWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            //注册Redis
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();

            //注册appsettings读取类
            services.AddSingleton(new AppsettingsHelper(Configuration));

            //数据库配置
            BaseDBConfig.ConnectionString= Configuration.GetSection("AppSettings:ConnectionString").Value;



            //jwt授权验证
            services.AddAuthorizationSetup();
            // var text = AppsettingsHelper.app(new string[] { "AppSettings", "ConnectionString" });
            // Console.WriteLine($"ConnectionString:{text}");
            //  Console.ReadLine();

       
            //注册接口文档程序Swagger
            services.AddSwaggerSetup();

            //注册服务日志
            services.AddLogSetup();

            services.AddControllers(option =>
            {
                //在启动服务中，注入全局异常 
                option.Filters.Add(typeof(GlobalExceptionsFilter));   
            });
        }

        /// <summary>
        ///     Autofac规则配置
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //获取所有控制器类型并使用属性注入
            builder.RegisterModule(new AutofacModuleRegister());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.ExceptionMiddleware(); //注册中间件（定义错误消息返回格式）

            //使用默认UI
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint($"/swagger/V1/swagger.json", "WebApi.Core V1");

            //    //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
            //    c.RoutePrefix = "";
            //});

            //Swagger使用自定义UI  https://blog.csdn.net/IT_rookie_newbie/article/details/124591376
            app.UseKnife4UI(c =>
            {

                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", "NETCore Api V1");

            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}



/*
   其实Startup算是程序真正的切入点.

ConfigureServices方法是用来把services(各种服务, 例如identity, sqlsugar，swagger等等包括第三方的, 
或者自己写的)加入(register)到container(asp.net core的容器)中去, 并配置这些services. 
这个container是用来进行dependency injection的(依赖注入). 所有注入的services(此外还包括一些框架已经注册好的services) 在以后写代码的时候,
都可以将它们注入(inject)进去. 例如上面的Configure方法的参数, app, env, loggerFactory都是注入进去的services.

Configure方法是asp.net core程序用来具体指定如何处理每个http请求的, 
例如我们可以让这个程序知道我使用路由规则来处理http请求, 那就调用app.UseRouting()这个方法就行
 */
