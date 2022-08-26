using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Caching.Redis
{

    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisCacheManager2
    {

        //获取 Reids 缓存值
        string GetValue(string key);

        //获取值，并序列化
        TEntity Get<TEntity>(string key);

         /// <summary>
         /// 保存(自定义时间)
         /// </summary>
         /// <param name="key"></param>
         /// <param name="value"></param>
         /// <param name="cacheTime"></param>
        void Set(string key, object value, TimeSpan cacheTime);

        /// <summary>
        /// 保存(固定时间)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetValue(string key, byte[] value);


        //判断是否存在
        bool Get(string key);

        //移除某一个缓存值
        void Remove(string key);

        //全部清除
        void Clear();
    }
}
