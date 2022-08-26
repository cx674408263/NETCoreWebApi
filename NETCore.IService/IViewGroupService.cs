using NETCore.IService.Base;
using NETCore.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.IService
{
    public interface IViewGroupService :IBaseService<ViewGroup>
    {

        /// <summary>
        /// 获取明细
        /// </summary>
        /// <returns></returns>
        Task<List<ViewGroup>> GetViewDtl();
       
    }
}
