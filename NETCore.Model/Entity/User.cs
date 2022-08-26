using SqlSugar;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace NETCore.Model.Entity
{
    ///<summary>
    ///用户表
    ///</summary>
    [SugarTable("User")]
    public partial class User
    {
        public User()
        {


        }
        /// <summary>
        ///用户ID
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int UserId { get; set; }

        /// <summary>
        ///用户名
        /// </summary>           
        public string UserName { get; set; }

        /// <summary>
        ///年龄
        /// </summary>           
        public int? Age { get; set; }

    }
}
