using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Model.Entity
{
    ///<summary>
    ///品牌实体类
    ///</summary>
    [SugarTable("BSBRAND")]
    public partial class Brand
    {
        public Brand()
        {


        }

        /// <summary>
        /// 用户ID
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int BRANDID { get; set; }

        /// <summary>
        /// 编码
        /// </summary>           
        public string CODE { get; set; }

        /// <summary>
        ///名称
        /// </summary>           
        public string NAME { get; set; }

        /// <summary>
        /// 备注
        /// </summary>           
        public string MEMO { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>           
        public DateTime CREATE_TIME { get; set; }
        /// <summary>
        ///创建人
        /// </summary>           
        public int? CREATOR { get; set; }
        /// <summary>
        /// 名称
        /// </summary>           
        public DateTime UP_TIME { get; set; }

        // <summary>
        ///修人ID
        /// </summary>           
        public int? UPOR { get; set; }

    }

}
