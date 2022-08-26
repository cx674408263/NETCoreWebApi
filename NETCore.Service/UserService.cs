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
    public class UserService : BaseServices<User>, IUserService
    {
        private readonly IUserRepository userDal;
        public UserService(IBaseRepository<User> baseRepository, IUserRepository userRepository) : base(baseRepository)
        {
            userDal = userRepository;
        }
    }
}
