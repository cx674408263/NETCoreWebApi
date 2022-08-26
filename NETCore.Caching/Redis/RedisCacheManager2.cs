using NETCore.Components.Helper;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Caching.Redis
{
    public class RedisCacheManager2 : IRedisCacheManager2
    {
        private readonly string redisConnenctionString;
        private readonly int redisDatabase;
        public volatile ConnectionMultiplexer redisConnection;

        private readonly object redisConnectionLock = new object();
        public RedisCacheManager2()
        {
            string redisConfiguration = AppsettingsHelper.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });//获取连接字符串
            string redisDatabaseConfiguration = AppsettingsHelper.app(new string[] { "AppSettings", "RedisCaching", "RedisDatabase" });//活动redis数据库
            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis配置为空", nameof(redisConfiguration));
            }
            this.redisConnenctionString = redisConfiguration;
            this.redisConnection = GetRedisConnection();
            this.redisDatabase = Convert.ToInt32(redisDatabaseConfiguration);


        }

        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 夹lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (this.redisConnection != null && this.redisConnection.IsConnected)
            {
                return this.redisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (redisConnectionLock)
            {
                if (this.redisConnection != null)
                {
                    //释放redis连接
                    this.redisConnection.Dispose();
                }
                try
                {
                    this.redisConnection = ConnectionMultiplexer.Connect(redisConnenctionString);
             
                }
                catch (Exception)
                {

                    throw new Exception("Redis服务未启用，请开启该服务");
                }
            }
            return this.redisConnection;
        }

        public void Clear()
        {
            foreach (var endPoint in this.GetRedisConnection().GetEndPoints())
            {
                var server = this.GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    redisConnection.GetDatabase(this.redisDatabase).KeyDelete(key);
                }
            }
        }

        public bool Get(string key)
        {
            return redisConnection.GetDatabase(this.redisDatabase).KeyExists(key);
        }

        public string GetValue(string key)
        {
            return redisConnection.GetDatabase(this.redisDatabase).StringGet(key);
        }

        public TEntity Get<TEntity>(string key)
        {
            var value = redisConnection.GetDatabase(this.redisDatabase).StringGet(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return SerializeHelper.Deserialize<TEntity>(value);
            }
            else
            {
                return default(TEntity);
            }
        }

        public void Remove(string key)
        {
            redisConnection.GetDatabase(this.redisDatabase).KeyDelete(key);
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                redisConnection.GetDatabase(this.redisDatabase).StringSet(key, SerializeHelper.Serialize(value), cacheTime);
            }
        }

        public bool SetValue(string key, byte[] value)
        {
            return redisConnection.GetDatabase(this.redisDatabase).StringSet(key, value, TimeSpan.FromSeconds(120));
        }
    }
}
