using NETCore.IRepository;
using NETCore.IRepository.Base;
using NETCore.IService;
using NETCore.Model.Entity;
using NETCore.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Service
{
    public class BrandService :BaseServices<Brand>, IBrandService
    {


        private readonly IBrandRepository brandDal;
        public BrandService(IBaseRepository<Brand> baseRepository, IBrandRepository brandRepository) : base(baseRepository)
        {
            this.brandDal = brandRepository;
        }
    }
}
