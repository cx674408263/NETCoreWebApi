using NETCore.Caching.Redis;
using NETCore.IRepository;
using NETCore.IRepository.Base;
using NETCore.IService;
using NETCore.Model.Entity;
using NETCore.Repository.Base;
using NETCore.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Service
{
    public class ViewGroupService : BaseServices<ViewGroup>, IViewGroupService
    {
        private readonly IViewGroupRepository viewDal;
        public ViewGroupService(IBaseRepository<ViewGroup> baseRepository, IViewGroupRepository viewRepository) : base(baseRepository)
        {
            this.viewDal = viewRepository;
        }

        [Caching(AbsoluteExpiration = 1)] ///AOP切面缓存
        /// <summary>
        /// 多表联合查询获取明细
        /// </summary>
        /// <returns></returns>
        public async Task<List<ViewGroup>> GetViewDtl()
        {
            return await viewDal.GetViewDtl();
        }
    }
}
