using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Twitter.DTOs;
using Twitter.Models;
using Twitter.Repositories;
using Twitter.Utilities;

namespace Twitter.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _user;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;

    public UserController(ILogger<UserController> logger,IMemoryCache Cache,
    IUserRepository user, IConfiguration config)
    {
        _logger = logger;
        _user = user;
        _config = config;
        _cache = Cache;

    }
     private long GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResDTO>> Login(
        [FromBody] UserLoginDTO Data
    )
    {
        var userCache = _cache.Get<User>(key: "user");
        if (userCache is null)
        {
            userCache = await _user.GetByEmail(Data.Email);
        }
        if (userCache is null)
            return NotFound();
        // var existingUser = await _user.GetByEmail(Data.Email);

        // if (existingUser is null)
        //     return NotFound();
        try
        {
            bool passwordVerify = BCrypt.Net.BCrypt.Verify(Data.Password, userCache.Password);

            if (!passwordVerify)
                return BadRequest("Incorrect password");

            var token = Generate(userCache);

            var res = new UserLoginResDTO
            {
                UserId = userCache.UserId,
                Email = userCache.Email,
                Username = userCache.Username,
                Token = token,
            };
            return Ok(res);
        }
        catch (Exception e)
        {
            Console.WriteLine(" error while verifying password: " + e.ToString());
            return Ok(" error while verifying password: " + e.ToString());
        }
    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(TwitterConstants.UserId, user.UserId.ToString()),
            new Claim(TwitterConstants.Username, user.Username),
            // new Claim(TwitterConstants.Email, user.Email),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UserCreateDto Data)
    {
        var toCreateuser = new User
        {
            Username = Data.Username.Trim(),
            Email = Data.Email.Trim(),
            Password = BCrypt.Net.BCrypt.HashPassword(Data.Password.Trim()),


        };

        var res = await _user.Create(toCreateuser);

        return StatusCode(StatusCodes.Status201Created, res);
    }
    [HttpGet]
    
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var allUsers = await _user.GetAll();
        return Ok(allUsers);
    }
    

    [HttpGet("{user_id}")]
    
    public async Task<ActionResult<User>> GetById(long user_id)
    {
        var user = await _user.GetById(user_id);
        if (user is null)
            return NotFound();
        return Ok(user);
    }



    [HttpPut("{user_id}")]
    [Authorize]
    public async Task<ActionResult> UpdateUser([FromRoute] long user_id,
    [FromBody] UserUpdateDto Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _user.GetById(user_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot update other users");

        var toUpdateItem = existingItem with
        {
            // Username = Data.Username is null ? existingItem.Username : Data.Username.Trim(),
            Username = Data.Username is null ? existingItem.Username : Data.Username.Trim(),

        };

        await _user.Update(toUpdateItem);

        return NoContent();
    }

}
