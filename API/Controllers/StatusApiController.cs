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
    [Route("api/status")]
    public class StatusApiController : ControllerBase
    {
        private readonly IStatusInterface _statusRepo;
        public StatusApiController(IStatusInterface status)
        {
            _statusRepo = status;
        }

        #region Get
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(string id="")
        {
            if (string.IsNullOrEmpty(id))   // Get All
            {
                List<t_Status> statusList = await _statusRepo.GetAll();
                return Ok(new {
                    success = true,
                    message = "Status List fetched Successfully",
                    data = statusList
                });
            }
            else    // Get One
            {
                t_Status status = await _statusRepo.GetOne(Convert.ToInt32(id));
                return Ok(new {
                    success = true,
                    message = "Status fetched Successfully",
                    data = status
                });
            }
        }
        #endregion
    }
}