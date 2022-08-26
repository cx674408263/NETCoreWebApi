using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Model.Entity
{
    ///<summary>
    ///用户表
    ///</summary>
    [SugarTable("VIEW_GROUP")]
    public partial class ViewGroup
    {
        public ViewGroup()
        {


        }
        /// <summary>
        ///用户ID
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        ///上级ID
        /// </summary>           
        public string PId { get; set; }

        /// <summary>
        ///标题
        /// </summary>           
        public string Title { get; set; }

        /// <summary>
        ///图标
        /// </summary>           
        public string icon { get; set; }

        /// <summary>
        ///链接地址
        /// </summary>           
        public string  href { get; set; }

    }
}
