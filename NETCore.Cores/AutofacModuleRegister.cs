using Autofac;
using Autofac.Extras.DynamicProxy;
using NETCore.Caching.AOP;
using System;
using System.Reflection;

namespace NETCore.Cores
{

    /// <summary>
    /// 注入容器
    /// </summary>

    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //注册RedisAOP   注入redisaop缓存
            builder.RegisterType<RedisCacheAOP>();


            /*构造函数注入*/
            //注册Service
            var assemblysServices = Assembly.Load("NETCore.Service");
            builder.RegisterAssemblyTypes(assemblysServices)
               .InstancePerDependency()     //瞬时单例
               .AsImplementedInterfaces()   ////自动以其实现的所有接口类型暴露（包括IDisposable接口）
               .EnableInterfaceInterceptors() //引用Autofac.Extras.DynamicProxy;
               .InterceptedBy(typeof(RedisCacheAOP));//可以放一个AOP拦截器集合

            //注册Repository
            var assemblysRepository = Assembly.Load("NETCore.Repository");
            builder.RegisterAssemblyTypes(assemblysRepository)
               .InstancePerDependency()   //瞬时单例
               .AsImplementedInterfaces() ////自动以其实现的所有接口类型暴露（包括IDisposable接口）
               .EnableInterfaceInterceptors();   //引用Autofac.Extras.DynamicProxy;

        }

    }
}


/*
  Autofac 的主要特性如下：

灵活的组件实例化：Autofac 支持自动装配，给定的组件类型 Autofac 自动选择使用构造函数注入或者属性注入，Autofac 还 可以基于 lambda 表达式创建实例，这使得容器非常灵活，很容易和其他的组件集成。 var defaultLog = new ConsoleLog();  builder.Register(c => new Connection(){ Log = c.ResolveOptional<ILog>() ?? defaultLog }); 大家知道 lambda 表达式并不是 在声明的时候的执行的，只有等到容器的 Resolve () 方法调用的时候，表达式才执行。表达式还有一个好处是不需要使用反射或者是使用 XML 语法来表 达。
资源管理的可视性：基于依赖注入容器构建的应用程序的动态性，意味着什么时候应该处理那些资源有点困难。Autofac 通过跟踪特 定作用域内的实例和依赖来解决这个问题（DeterministicDisposal）。IDisposable 接口接口是把双刃剑，既是一个老孙手上 的金箍棒，也是老孙头上的魔咒，有一种明确的方式告诉那一部分应该被清理，但是一个组件要何时处理并不是很容易确定的事情，比如说一个服务可以有多个实现 的时候就变得很糟糕，组件的创建上（GOF 的创建型设计模式）有的是通过工厂方式创建的，有的是单件方式创建的，有些需要被清理，有些却不需要清理。组件 的使用者无法知道是否把转换为 IDisposable 接口调用它的 Disposal 方法。Autofac 通过容器来跟踪组件的资源管理。对于不需要清理的 对象，例如 Console.Out，我们调用 ExternallyOwned () 方法告诉容器不用清理。细粒度的组件生命周期管理：应用程序中通常可以存 在一个应用程序范围的容器实例，在应用程序中还存在大量的一个请求的范围的对象，例如一个 HTTP 请求，一个 IIS 工作者线程或者用户的会话结束时结束。 通过嵌套的容器实例和对象的作用域使得资源的可视化。
Autofac 的设计上非常务实，这方面更多是为我们这些容器的使用者考虑：
组件侵入性为零：组件不需要去引用 Autofac。
灵活的模块化系统：通过模块化组织你的程序，应用程序不用纠缠于复 杂的 XML 配置系统或者是配置参数。
自动装配：可以是用 lambda 表达式注册你的组件，autofac 会根据需要选择构造函数或者属 性注入
XML 配置文件的支持：XML 配置文件过度使用时很丑陋，但是在发布的时候通常非常有用
组件的多服务支持：许 多设计师喜欢使用细粒度的接口来控制依赖 ， autofac 允许一个组件提供多个服务。

 */
