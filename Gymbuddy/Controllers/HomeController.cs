using Gymbuddy.Entities;
using Gymbuddy.Models;
using Gymbuddy.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Post([FromForm] PostModel obj)
        {
            int Id = Convert.ToInt32(obj.Id);
            Post model = new Post();
            if (obj.file != null)
            {

                model.ImageUrl = _fileManager.postUpload(obj.file);
            }
            model.Likes = 0;
            model.Description = obj.description;
            model.UserId = Id;
            model.dateCreated = DateTime.UtcNow;
            _db.Posts.Add(model);
            _db.SaveChanges();
            return Ok(model);
        }
        [Authorize]
        [HttpGet("GetPosts")]
        public IActionResult GetPosts()
        {
            var posts = _db.Posts.Include(x => x.User).ToList();
            return Ok(posts);
        }
        [HttpGet("getFilteredUsers")]
        public IActionResult GetFilteredUsers(string search)
        {
            var users = _db.Users.Where(x=>x.FirstName.ToLower().Contains(search.ToLower()) || x.LastName.ToLower().Contains(search.ToLower()) || x.UserName.ToLower().Contains(search.ToLower())).ToList();
            return Ok(users);
        }
    }
}
