using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace library_project.Controllers
{
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        public LoanController(Database db)
        {
            Db = db;
        }

        // GET api/Loan/user01
        [HttpGet("{username}")]
        public async Task<IActionResult> GetAll(string username)
        {
            await Db.Connection.OpenAsync();
            var query = new Loan(Db);
            var result = await query.GetAllLoans(username);
            return new OkObjectResult(result);
        }

        public Database Db { get; }
    }
}