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
 ���Program�ǳ�������, ������������, ����Ϊasp.net core applicationʵ�ʾ��ǿ���̨����(console application).

����һ������asp.net core ��ؿ��console application. 

Main���������������Ҫ���������ú����г����.

��Ϊ���ǵ�web������Ҫһ������, ���� BuildWebHost��������ʹ�����һ��WebHostBuilder. �������ǻ���ҪWeb Server.

asp.net core �Դ�������http servers, һ����WebListener, ��ֻ������windowsϵͳ, ��һ����kestrel, ���ǿ�ƽ̨��.

kestrel��Ĭ�ϵ�web server, ����ͨ��UseKestrel()������������õ�.

�������ǿ�����ʱ��ʹ�õ���IIS Express, ����UseIISIntegration()�������������IIS Express, ����ΪKestrel��Reverse Proxy server����.

�����windows�������ϲ���Ļ�, ��Ӧ��ʹ��IIS��ΪKestrel�ķ�����������������ʹ�������.

�����linux�ϵĻ�, ����ʹ��apache, nginx�ȵȵ���Ϊkestrel��proxy server.

��ȻҲ���Ե���ʹ��kestrel��Ϊweb ������, ����ʹ��iis��Ϊreverse proxy�����кܶ����ŵ��: ����,IIS���Թ�������, ����֤��, �������ʱ�Զ�������.

UseStartup<Startup>(), ��仰��ʾ�ڳ���������ʱ��, ���ǻ����Startup�����.

Build()��֮�󷵻�һ��ʵ����IWebHost�ӿڵ�ʵ��(WebHostBuilder), Ȼ�����Run()�ͻ�����Web����, ������ֹ������õ��߳�, ֱ������ر�.
 */