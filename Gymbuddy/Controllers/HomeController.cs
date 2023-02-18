using Gymbuddy.Entities;
using Gymbuddy.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Gymbuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly DB _db;
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly FileManager _fileManager;


        public HomeController(DB db, IWebHostEnvironment hostEnvironment, FileManager fileManager)
        {
            _db = db;
            _hostEnviroment = hostEnvironment;
            _fileManager = fileManager;
        }
        [HttpPost("Post")]
        public IActionResult Post([FromForm] string description, IFormFile file)
        {

            Post model = new Post();
            if (file != null)
            {

                model.ImageUrl = _fileManager.postUpload(file);
            }
            model.Likes = 0;
            model.Description = description;
            model.UserId = 7;
            model.dateCreated = DateTime.UtcNow;
            _db.Posts.Add(model);
            _db.SaveChanges();
            return Ok();
        }
        [HttpGet("GetPosts")]
        public IActionResult GetPosts()
        {
            var posts = _db.Posts.ToList();
            return Ok(posts);
        }
    }
}
