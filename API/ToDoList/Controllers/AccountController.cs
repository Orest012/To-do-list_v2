using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;
using ToDoList.DTO;
using ToDoList.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;


    public AccountController(ApplicationDbContext appDbContext, IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto request)
    {
        var respond = _accountService.Login(request);
        if (respond == null)
        {
            return Unauthorized("Невірний email або пароль");
        }
        return Ok(new { Token = respond });

    }


    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegisterDto request)
    {
        var respond = _accountService.Register(request);
        if (respond == null)
        {
            return BadRequest("Користувач з таким email вже існує.");
        }


        return Ok(new { Token = respond });
    }
}
