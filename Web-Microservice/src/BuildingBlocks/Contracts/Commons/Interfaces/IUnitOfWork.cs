using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Commons.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext: DbContext
    {
        Task<int> CommitAsync();
    }
}
