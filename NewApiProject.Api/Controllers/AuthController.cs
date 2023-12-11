using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewApiProject.Api.Entites;
using NewApiProject.Api.Repositories;
using NewApiProject.Api.Services;

namespace NewApiProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly PasswordHasher<User> _Hasher;

        public AuthController(IUserRepository repository, PasswordHasher<User> _hasher)
        {
            _repository = repository;
            _Hasher = _hasher;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody]User userObj) 
        { 
            if(userObj == null) 
            {
                return BadRequest();
            }


            var user = await _repository.GetUser(userObj);

            if (user == null) 
            { 
                return NotFound( new { Message = "User Not Found!"});
            }

            if (_Hasher.VerifyHashedPassword(user, user.Password, userObj.Password) == 0)
            {
                return BadRequest(new { Message = "Wrong Username Or Wrong Password" });
            }


            user.Token = JwtService.CreateJwtTOKEN(user);

            return Ok( new {
                Token = user.Token,
                Message = $"Logged in User: {userObj.Username}"
            });
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj) 
        { 
            if(userObj == null) 
            {
                return BadRequest();
            }

            userObj.Password = _Hasher.HashPassword(userObj, userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";

            await _repository.RegisterUser(userObj);

            return Ok( new {Message = $"User: {userObj.Username} is registered"});
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers() 
        { 
            return Ok(await _repository.GetAllUsers());
        
        }
    }
}
