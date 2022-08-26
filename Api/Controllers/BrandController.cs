using Microsoft.AspNetCore.Mvc;
using NETCore.Caching.Redis;
using NETCore.IRepository;
using NETCore.IRepository.Base;
using NETCore.IService;
using NETCore.Model.Entity;
using NETCore.Repository;
using NETCore.Repository.Sugar;
using NETCore.Service;
using org.apache.zookeeper;
using System;
using System.Threading.Tasks;

namespace NETCoreWebApi.Controllers
{

    /// <summary>
    /// 品牌接口
    /// </summary>
    public class BrandController : BaseController
    {

        private readonly IViewGroupService _viewService;
        private readonly IBrandService _brandService;
        private readonly IRedisCacheManager _redisCacheManager;   //--引用redis  
          


        public BrandController(IViewGroupService viewService,IBrandService brandService, IRedisCacheManager redisCacheManager)
        {

            this._viewService = viewService;
            this._brandService = brandService;
            this._redisCacheManager =redisCacheManager;

        }


        /// <summary>
        /// 获取Brand实体
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        // [ApiExplorerSettings(IgnoreApi = true)]  //如果不想显示某些接口，直接在controller 上，或者action 上，增加特性
        [HttpPost]
        public IActionResult Brand(Brand brand)
        {
            return Ok(brand);
        }


        /// <summary>
        /// 品牌信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBrand(int id)
        {
         

            Brand brand = await _brandService.QueryByID(id);

            return Ok(brand);

        }

        /// <summary>
        ///新增品牌信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddBrand(Brand brand)
        {

            bool status = await _brandService.Add(brand);

            return Ok(status);

        }


        /// <summary>
        ///更新品牌信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateBrand(Brand brand)
        {
           // IBrandService brandService = new BrandService();

            bool status = await _brandService.Update(brand);

            return Ok(status);

        }



        /// <summary>
        ///获取多表明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetDtl()
        {

            //   IViewGroupService _viewService = new ViewGroupService();

            var view = await _viewService.GetViewDtl();

            return Ok(view);

        }


        /// <summary>
        /// 测试Redis
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Redis(int id)
        {

            var key = $"Redis{id}";
            ViewGroup view = new ViewGroup();
            if (_redisCacheManager.Get<object>(key) != null)
            {
                view = _redisCacheManager.Get<ViewGroup>(key);
            }
            else
            {
                view = await _viewService.QueryByID(id);

                _redisCacheManager.Set(key, view, TimeSpan.FromHours(2));//缓存2小时
            }

            return Ok(view);
        }

        /// <summary>
        /// 测试aop缓存redis
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AopTest(int id)
        {
            var view = await _viewService.QueryByID(id);
            return Ok(view);
        }

    }
}
