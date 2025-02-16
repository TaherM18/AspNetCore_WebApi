using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IStateInterface
    {
        public Task<t_State> GetOne(int id);
        public Task<List<t_State>> GetAll();
    }
}