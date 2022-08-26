using NETCore.IRepository.Base;
using NETCore.IService.Base;
using NETCore.Repository.Base;
using System.Threading.Tasks;

namespace NETCore.Service.Base
{
    public class BaseServices<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        private readonly IBaseRepository<TEntity> baseDal;


        public BaseServices(IBaseRepository<TEntity> baseRepository)
        {
            this.baseDal = baseRepository;
        }

        /// <summary>
        /// 写入实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Add(TEntity model)
        {
            return await baseDal.Add(model);
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>

        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await baseDal.DeleteByIds(ids);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public async Task<TEntity> QueryByID(object objId)
        {
            return await baseDal.QueryByID(objId);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity model)
        {
            return await baseDal.Update(model);
        }
    }
}
