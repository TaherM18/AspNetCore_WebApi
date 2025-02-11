using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactSesApiController : ControllerBase
    {
        private readonly IContactInterface _contact;
        public ContactSesApiController(IConfiguration configuration, IContactInterface contact)
        {
            _contact = contact;
        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string userid = HttpContext.Request.Query["userid"].ToString();
            List<t_Contact> list;
            if (userid != "")
            {
                list = await _contact.GetAllByUser(userid);
            }
            else
            {
                list = await _contact.GetAll();
            }
            return Ok(list);
        }
        #endregion GetAll


        #region GetById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(string id)
        {
            var contact = await _contact.GetOne(id);
            if (contact == null)
                return BadRequest(new { success = false, message = "There was no contact found" });
            return Ok(contact);
        }
        #endregion GetById


        #region Add
        [HttpPost]
        public async Task<IActionResult> AddContact([FromForm] t_Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }
            if (contact.ContactPicture != null && contact.ContactPicture.Length > 0)
            {
                // Save the uploaded file
                var fileName = contact.c_Email + Path.GetExtension(contact.ContactPicture.FileName);
                var filePath = Path.Combine("../MVC/wwwroot/contact_images", fileName);
                contact.c_Image = fileName;
                System.IO.File.Delete(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    contact.ContactPicture.CopyTo(stream);
                }
            }
            var status = await _contact.Add(contact);
            if (status == 1)
            {
                return Ok(new { success = true, message = "contact Insterted Successfully!!!!!" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There was some error while adding the contact" });
            }
        }
        #endregion Add


        #region Update
        [HttpPut]
        public async Task<IActionResult> UpdateContact([FromForm] t_Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }
            if (contact.ContactPicture != null && contact.ContactPicture.Length > 0)
            {
                var fileName = contact.c_Email + Path.GetExtension(contact.ContactPicture.FileName);
                var filePath = Path.Combine("../MVC/wwwroot/contact_images", fileName);
                contact.c_Image = fileName;
                System.IO.File.Delete(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    contact.ContactPicture.CopyTo(stream);
                }
            }
            int status = await _contact.Update(contact);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Contact Updated Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There was some error while Update Contact" });
            }
        }
        #endregion Update


        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {
            int status = await _contact.Delete(id);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Contact Deleted Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There Is Some Error while Delete Contact" });
            }
        }
        #endregion Delete
    }
}