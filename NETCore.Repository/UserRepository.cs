using NETCore.IRepository;
using NETCore.Model.Entity;
using NETCore.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

    }
}
