﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Common.Authorization;

namespace Boilerplate.Common.Types.Interfaces
{
    public interface IRepository<T> : Utils.IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity, UserInfo userInfo = null);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, UserInfo userInfo = null);
        Task<T> UpdateAsync(T entity, UserInfo userInfo = null);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, UserInfo userInfo = null);
    }
}
