using NETCore.IRepository;
using NETCore.Model.Entity;
using NETCore.Repository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Repository
{

    public class ViewGroupRepository : BaseRepository<ViewGroup>, IViewGroupRepository
    {



        /// <summary>
        /// 获取明细
        /// </summary>
        /// <returns></returns>
        public async Task<List<ViewGroup>> GetViewDtl()
        {

            List<ViewGroup> page = new List<ViewGroup>();

            //page = await Db.Queryable<ViewGroup, ViewGroupBrand>((vg, vgs) =>
            //                 new JoinQueryInfos(JoinType.Inner, vg.Id == vgs.GroupId))
            //             .Select((vg, vgs)=>new ViewGroup
            //             { 
            //                Id=vgs.GroupId,
            //                href=vg.href,
            //                icon=vg.icon,
            //                PId=vg.PId,
            //                Tittle=vg.Tittle
            //             }).ToList();


            page = await Task.Run(() =>
                         Db.Queryable<ViewGroup, ViewGroupBrand>((vg, vgs) => new JoinQueryInfos(JoinType.Inner, vg.Id == vgs.GroupId))
                           .Select((vg, vgs) => new ViewGroup
                           {
                               Id = vg.Id,
                               PId = vg.PId,
                               href = vg.href,
                               icon = vg.icon,
                               Title = vg.Title

                           }).ToList()
                   );



            return page;

        }




    }
}
