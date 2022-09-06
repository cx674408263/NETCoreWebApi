
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
            
            //ע��Redis
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();

            //ע��appsettings��ȡ��
            services.AddSingleton(new AppsettingsHelper(Configuration));

            //���ݿ�����
            BaseDBConfig.ConnectionString= Configuration.GetSection("AppSettings:ConnectionString").Value;



            //jwt��Ȩ��֤
            services.AddAuthorizationSetup();
            // var text = AppsettingsHelper.app(new string[] { "AppSettings", "ConnectionString" });
            // Console.WriteLine($"ConnectionString:{text}");
            //  Console.ReadLine();

       
            //ע��ӿ��ĵ�����Swagger
            services.AddSwaggerSetup();

            //ע�������־
            services.AddLogSetup();

            services.AddControllers(option =>
            {
                //�����������У�ע��ȫ���쳣 
                option.Filters.Add(typeof(GlobalExceptionsFilter));   
            });
        }

        /// <summary>
        ///     Autofac��������
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //��ȡ���п��������Ͳ�ʹ������ע��
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

            app.ExceptionMiddleware(); //ע���м�������������Ϣ���ظ�ʽ��

            //ʹ��Ĭ��UI
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint($"/swagger/V1/swagger.json", "WebApi.Core V1");

            //    //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
            //    c.RoutePrefix = "";
            //});

            //Swaggerʹ���Զ���UI  https://blog.csdn.net/IT_rookie_newbie/article/details/124591376
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
   ��ʵStartup���ǳ��������������.

ConfigureServices������������services(���ַ���, ����identity, sqlsugar��swagger�ȵȰ�����������, 
�����Լ�д��)����(register)��container(asp.net core������)��ȥ, ��������Щservices. 
���container����������dependency injection��(����ע��). ����ע���services(���⻹����һЩ����Ѿ�ע��õ�services) ���Ժ�д�����ʱ��,
�����Խ�����ע��(inject)��ȥ. ���������Configure�����Ĳ���, app, env, loggerFactory����ע���ȥ��services.

Configure������asp.net core������������ָ����δ���ÿ��http�����, 
�������ǿ������������֪����ʹ��·�ɹ���������http����, �Ǿ͵���app.UseRouting()�����������
 */
