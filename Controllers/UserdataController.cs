using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace library_project.Controllers
{
    [Route("api/[controller]")]
    public class UserdataController : ControllerBase
    {
        public UserdataController(Database db)
        {
            Db = db;
        }

        // GET api/User/5
        [HttpGet("User/{username}")]
        public async Task<IActionResult> GetOne(string username)
        {
            await Db.Connection.OpenAsync();
            var query = new Userdata(Db);
            var result = await query.FindOneAsync(username);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);

        }

        // PUT api/User/5
        [HttpPut("User/{username}")]
        public async Task<IActionResult> PutOne(string username, [FromBody] Userdata body)
        {
            await Db.Connection.OpenAsync();
            var query = new Userdata(Db);
            var result = await query.FindOneAsync(username);
            result.firstname = body.firstname;
            result.lastname = body.lastname;
            result.phone = body.phone;
            result.streetaddress = body.streetaddress;
            result.postalcode = body.postalcode;
            if (result is null)
                return new NotFoundResult();
            int updateTest = await result.UpdateAsync();
            if (updateTest == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkObjectResult(updateTest);
            }
        }

        // PUT api/Password/5
        [HttpPut("Password/{username}")]
        public async Task<IActionResult> ChangePassword(string username, [FromBody] Userdata body)
        {
            await Db.Connection.OpenAsync();
            var query = new Userdata(Db);
            var result = await query.FindOneAsync(username);
            result.password = BCrypt.Net.BCrypt.HashPassword(body.password);
            if (result is null)
                return new NotFoundResult();
            int updateTest = await result.ChangePassword();
            if (updateTest == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkObjectResult(updateTest);
            }
        }

                        // PUT api/Password/5
        [HttpPut("Image/{username}")]
        public async Task<IActionResult> ChangeImage(string username, [FromBody] Userdata body)
        {
            await Db.Connection.OpenAsync();
            var query = new Userdata(Db);
            var result = await query.FindOneAsync(username);
            result.image = body.image;
            if (result is null)
                return new NotFoundResult();
            int updateTest = await result.ChangeImage();
            if (updateTest == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkObjectResult(updateTest);
            }
        }

        // DELETE api/User/5
        [HttpDelete("User/{username}")]
        public async Task<IActionResult> DeleteOne(string username)
        {
            await Db.Connection.OpenAsync();
            var query = new Userdata(Db);
            var result = await query.FindOneAsync(username);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkObjectResult(result);
        }


        public Database Db { get; }
    }
}