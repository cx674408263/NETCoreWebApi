using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Caching.Redis
{

    /// <summary>
    /// 更新Redis缓存接口
    /// 在之前的redis缓存博客中我们定义了redis操作接口和实现，在实际项目开发中，我又对它进行了修改，主要是增加了异步和批量删除的接口。
    /// 修改Common下的Redis文件夹的IRedisCacheManager文件和RedisCacheManager增加以下接口和方法。
    /// </summary>

    public interface IRedisCacheManager
    {
        #region AOP 更新Redis缓存接口
        Task<string> GetValueAsync(string key);

        //获取值，并序列化
        Task<TEntity> GetAsync<TEntity>(string key);

        //保存
        Task SetAsync(string key, object value, TimeSpan cacheTime);

        //判断是否存在
        Task<bool> GetAsync(string key);

        //移除某一个缓存值
        Task RemoveAsync(string key);

        //根据关键字移除
        Task RemoveByKey(string key);

        //全部清除
        Task ClearAsync();
        #endregion




        void Remove(string key);



        //获取 Reids 缓存值
        string GetValue(string key);



        void Set(string key, object value, TimeSpan cacheTime);

         bool Get(string key);

        TEntity Get<TEntity>(string key);


    }
}
