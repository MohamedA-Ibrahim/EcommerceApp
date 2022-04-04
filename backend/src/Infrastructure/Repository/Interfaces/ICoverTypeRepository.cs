using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType category);
    }
}
