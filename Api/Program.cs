using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });


        public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS
  .ConfigureWebHostDefaults(webBuilder =>
  {
      webBuilder.UseStartup<Startup>();
  });
    }
}
/*
 这个Program是程序的入口, 看起来很眼熟, 是因为asp.net core application实际就是控制台程序(console application).

它是一个调用asp.net core 相关库的console application. 

Main方法里面的内容主要是用来配置和运行程序的.

因为我们的web程序需要一个宿主, 所以 BuildWebHost这个方法就创建了一个WebHostBuilder. 而且我们还需要Web Server.

asp.net core 自带了两种http servers, 一个是WebListener, 它只能用于windows系统, 另一个是kestrel, 它是跨平台的.

kestrel是默认的web server, 就是通过UseKestrel()这个方法来启用的.

但是我们开发的时候使用的是IIS Express, 调用UseIISIntegration()这个方法是启用IIS Express, 它作为Kestrel的Reverse Proxy server来用.

如果在windows服务器上部署的话, 就应该使用IIS作为Kestrel的反向代理服务器来管理和代理请求.

如果在linux上的话, 可以使用apache, nginx等等的作为kestrel的proxy server.

当然也可以单独使用kestrel作为web 服务器, 但是使用iis作为reverse proxy还是有很多有优点的: 例如,IIS可以过滤请求, 管理证书, 程序崩溃时自动重启等.

UseStartup<Startup>(), 这句话表示在程序启动的时候, 我们会调用Startup这个类.

Build()完之后返回一个实现了IWebHost接口的实例(WebHostBuilder), 然后调用Run()就会运行Web程序, 并且阻止这个调用的线程, 直到程序关闭.
 */