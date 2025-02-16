using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IStatusInterface
    {
        public Task<t_Status> GetOne(int id);
        public Task<List<t_Status>> GetAll();
    }
}