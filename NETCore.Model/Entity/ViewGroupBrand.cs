using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Model.Entity
{
    ///<summary>
    ///用户表
    ///</summary>
    [SugarTable("VIEW_GROUP_BAND")]
    public partial class ViewGroupBrand
    {
        public ViewGroupBrand()
        {


        }
        /// <summary>
        ///视图ID
        /// </summary>           

        public int Id { get; set; }

        /// <summary>
        ///分组ID
        /// </summary>           
        public int  GroupId { get; set; }

    }
}
