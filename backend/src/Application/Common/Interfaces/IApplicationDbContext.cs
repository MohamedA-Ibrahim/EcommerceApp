using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        //Todo: Add Dbsets
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
