using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace NETCore.Loging.Log4
{
    public static class LogSetup
    {
        /// <summary>
        /// log4net 仓储库
        /// </summary>
        public static ILoggerRepository repository { get; set; }

        public static void AddLogSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //log注入ILoggerHelper
            services.AddSingleton<LogHelper>();


            //log4net
            repository = LogManager.CreateRepository("");//需要获取日志的仓库名，也就是你的当然项目名
            XmlConfigurator.Configure(repository, new FileInfo("Log4net.config"));//指定配置文件，

        }
    }

}
