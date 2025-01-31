using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Internal;
using ToDoList.Data;

namespace ToDoList.Controllers
{
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        public HomeController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Get() {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Не вдалося ідентифікувати користувача");

            return Ok("користувач має id " + userId);
        }
    }
}
