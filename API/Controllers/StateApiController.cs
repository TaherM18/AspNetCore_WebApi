using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/state")]
    public class StateApiController : ControllerBase
    {
        private readonly IStateInterface _stateRepo;
        public StateApiController(IStateInterface state)
        {
            _stateRepo = state;
        }

        #region Get
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(string id="")
        {
            if (string.IsNullOrEmpty(id))   // Get All
            {
                List<t_State> stateList = await _stateRepo.GetAll();
                return Ok(new {
                    success = true,
                    message = "State List fetched Successfully",
                    data = stateList
                });
            }
            else    // Get One
            {
                t_State state = await _stateRepo.GetOne(Convert.ToInt32(id));
                return Ok(new {
                    success = true,
                    message = "State fetched Successfully",
                    data = state
                });
            }
        }
        #endregion
    }
}