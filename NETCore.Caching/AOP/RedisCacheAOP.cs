﻿using Castle.DynamicProxy;
using NETCore.Caching.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Caching.AOP
{
    /// <summary>
    /// 面向切面的缓存使用
    /// </summary>
    public class RedisCacheAOP : CacheAOPbase
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly IRedisCacheManager _cache;
        public RedisCacheAOP(IRedisCacheManager cache)
        {
            _cache = cache;
        }

        //Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
        //代码已经合并 ，学习pr流程
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
            {
                invocation.Proceed();
                return;
            }
            //对当前方法的特性验证

            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) is CachingAttribute qCachingAttribute)
            {
                //获取自定义缓存键
                var cacheKey = qCachingAttribute.CustomKeyValue ?? CustomCacheKey(invocation);
                //注意是 string 类型，方法GetValue
                var cacheValue = _cache.GetValue(cacheKey);
                if (cacheValue != null)
                {
                    if (qCachingAttribute.IsDelete)
                    {
                        //删除Redis里面的数据
                        _cache.Remove(cacheKey);
                    }
                    else
                    {
                        //将当前获取到的缓存值，赋值给当前执行方法
                        Type returnType;
                        if (typeof(Task).IsAssignableFrom(method.ReturnType))
                        {
                            returnType = method.ReturnType.GenericTypeArguments.FirstOrDefault();
                        }
                        else
                        {
                            returnType = method.ReturnType;
                        }
                        dynamic _result = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, returnType);
                        invocation.ReturnValue = (typeof(Task).IsAssignableFrom(method.ReturnType)) ? Task.FromResult(_result) : _result;
                        return;
                    }
                }
                //去执行当前的方法
                invocation.Proceed();

                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey) && qCachingAttribute.IsDelete == false)
                {
                    object response;

                    //Type type = invocation.ReturnValue?.GetType();
                    var type = invocation.Method.ReturnType;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        var resultProperty = type.GetProperty("Result");
                        response = resultProperty.GetValue(invocation.ReturnValue);
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    if (response == null) response = string.Empty;

                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(qCachingAttribute.AbsoluteExpiration));
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }
    }
}
