using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IDistrictInterface
    {
        public Task<t_District> GetOne(int id);
        public Task<List<t_District>> GetAll();
        public Task<List<t_District>> GetAllByState(int stateId);
    }
}