using NETCore.IRepository.Base;
using NETCore.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.IRepository
{
    public interface IViewGroupRepository : IBaseRepository<ViewGroup>
    {

        /// <summary>
        /// 获取明细
        /// </summary>
        /// <returns></returns>
        Task<List<ViewGroup>> GetViewDtl();
    }
}
