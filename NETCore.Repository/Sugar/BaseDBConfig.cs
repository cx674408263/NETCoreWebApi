using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Repository.Sugar
{
   public class BaseDBConfig
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }
      
        /// <summary>
        ///数据库类型
        /// </summary>
        public static string SqlType { get; set; }
    }
}
