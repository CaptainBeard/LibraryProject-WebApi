using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace library_project.Controllers
{
    [Route("api/[controller]")]
    public class BookdataController : ControllerBase
    {
        public BookdataController(Database db)
        {
            Db = db;
        }

        // GET api/Bookdata
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Bookdata(Db);
            var result = await query.GetAllBookData();
            return new OkObjectResult(result);
        }

        // GET api/Bookdata/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Bookdata(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
        public Database Db { get; }
    }
}