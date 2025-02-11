using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserInterface _userRepo;

        public UserApiController(IUserInterface userInterface)
        {
            _userRepo = userInterface;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] t_User user)
        {
            
            if (user.ProfilePicture != null && user.ProfilePicture.Length > 0)
            {
                Directory.CreateDirectory("../MVC/wwwroot/profile_images");

                var fileName = user.c_Email + Path.GetExtension(user.ProfilePicture.FileName);

                var filePath = Path.Combine("../MVC/wwwroot/profile_images", fileName);
                user.c_Image = fileName;
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    user.ProfilePicture.CopyTo(stream);
                }
            }

            Console.WriteLine("user.c_username: " + user.c_UserName);
            var status = await _userRepo.Register(user);

            if (status == 1)
            {
                return Ok(new { success = true, message = "User Registered" });
            }
            else if (status == 0)
            {
                return Ok(new { success = false, message = "User Already Exist" });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "There was some error while Registration",
                });

            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] vm_Login user)
        {
            t_User UserData = await _userRepo.Login(user);
            if (UserData.c_UserId != 0)
            {
                return Ok(new
                {
                    success = true,
                    message = "Login Success",
                    UserData
                });
            }
            else
            {
                return Ok(new { success = false, message = "Invalid email or password", UserData = UserData });
            }
        }

    }
}
