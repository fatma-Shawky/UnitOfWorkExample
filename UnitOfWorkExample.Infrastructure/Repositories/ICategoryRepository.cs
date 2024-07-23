using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWorkExample.core.Entities;
using UnitOfWorkExample.Core.Interfaces;

namespace UnitOfWorkExample.Infrastructure.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
    }

}
