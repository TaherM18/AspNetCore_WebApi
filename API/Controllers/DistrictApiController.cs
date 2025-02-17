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
    [Route("api/district")]
    public class DistrictApiController : ControllerBase
    {
        private readonly IDistrictInterface _districtRepo;
        public DistrictApiController(IDistrictInterface district)
        {
            _districtRepo = district;
        }

        #region Get
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetAll(string id="")
        {
            if (string.IsNullOrEmpty(id))   // Get All
            {
                List<t_District> districtList = await _districtRepo.GetAll();
                return Ok(new {
                    success = true,
                    message = "District List fetched Successfully",
                    data = districtList
                });
            }
            else    // Get One
            {
                t_District district = await _districtRepo.GetOne(Convert.ToInt32(id));
                return Ok(new {
                    success = true,
                    message = "District fetched Successfully",
                    data = district
                });
            }
        }
        #endregion Get


        #region GetByState
        [HttpGet]
        public async Task<IActionResult> GetByState()
        {
            string stateId = HttpContext.Request.Query["stateId"].ToString();
            if (!string.IsNullOrEmpty(stateId))
            {
                List<t_District> districtList = await _districtRepo.GetAllByState(Convert.ToInt32(stateId));
                return Ok(new {
                    success = true,
                    message = "District List for State fetched Successfully",
                    data = districtList
                });
            }
            else
            {
                return BadRequest(new {
                    success = false,
                    message = "Invalid stateId"
                });
            }
        }
        #endregion GetByState
    }
}