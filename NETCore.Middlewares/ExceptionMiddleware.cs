using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NETCore.Components.Helper;
using NETCore.Loging;
using System;
using System.Threading.Tasks;

namespace NETCore.Middlewares
{

    /// <summary>
    /// 自定义全局消息返回过滤中间件 
    /// </summary>
    /// 
    /*
     
     有的时候，接口请求会返回一些系统的状态码，如404,401,403等，我们会希望自定义这些返回消息，这个时候我们可以自定义一个中间件来在消息返回之前处理消息。
   
     */
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LogHelper _logger;
        public ExceptionMiddleware(RequestDelegate next, LogHelper logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex); // 日志记录
                await HandleExceptionAsync(httpContext, ex.Message);
            }
            finally
            {
                var statusCode = httpContext.Response.StatusCode;
                var msg = "";
                switch (statusCode)
                {
                    case 401:
                        msg = "未授权";
                        break;
                    case 403:
                        msg = "拒绝访问";
                        break;
                    case 404:
                        msg = "未找到服务";
                        break;
                    case 405:
                        msg = "405 Method Not Allowed";
                        break;
                    case 502:
                        msg = "请求错误";
                        break;
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(httpContext, msg);
                }
            }
        }
        ///
        private async Task HandleExceptionAsync(HttpContext httpContext, string msg)
        {
            ErrorModel error = new ErrorModel
            {
                code = httpContext.Response.StatusCode,
                msg = msg
            };
            var result = JsonHelper.toJson(error);
            httpContext.Response.ContentType = "application/json;charset=utf-8";
            await httpContext.Response.WriteAsync(result).ConfigureAwait(false);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder ExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }


    public class ErrorModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; } = 500;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 错误详情
        /// </summary>
        public string detail { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

}
