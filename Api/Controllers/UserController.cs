using NETCore.Loging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Components.Helper;
using NETCore.Model;
using NETCore.Model.Entity;
using NETCore.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreWebApi.Controllers
{
    /// <summary>
    /// 用户信息接口
    /// </summary>

    public class UserController : BaseController
    {


        private readonly LogHelper _logger;

        public UserController(LogHelper loggerHelper)
        {
            _logger = loggerHelper;
        }

        /// <summary>
        /// 接口测试Hello
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("Hello World");
        }


        /// <summary>
        /// 获取User实体
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
       // [ApiExplorerSettings(IgnoreApi = true)]  //如果不想显示某些接口，直接在controller 上，或者action 上，增加特性
        [HttpPost]
        public IActionResult User(User user)
        {
            return Ok(user);
        }


        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string role)
        {
            string jwtStr = string.Empty;
            bool suc = false;

            if (role != null)
            {
                // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
                TokenModel tokenModel = new TokenModel { Uid = "abcde", Role = role };
                jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

        /// <summary>
        /// 测试日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LogTest()
        {
            _logger.Error(typeof(UserController), "这是错误日志", new Exception("123"));
            _logger.Debug(typeof(UserController), "这是bug日志");
            //throw new System.IO.IOException();
            return Ok();
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Throw()
        {
            throw new System.IO.IOException();
        }
    }
}
