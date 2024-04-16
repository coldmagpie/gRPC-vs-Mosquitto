using AutoMapper;
using DataAccess.Repositories;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using User.DataAccess.Models;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(ILogger<UserController>logger, IUserRepository userRepository, IMapper mapper) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    [HttpPost("add")]
    public async Task<ActionResult> Register([FromBody]UserDto dto)
    {
        var user = _mapper.Map<UserModel>(dto);
        await _userRepository.AddAsync(user);
        return Ok("user created");
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("user not found");
        }
        var userDto = _mapper.Map<UserDto>(user);
        return Ok(userDto);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        if (users is null)
        {
            return Ok(new List<UserModel>());
        }
        return Ok(users);
    }

    [HttpPut("update")]
    public async Task<ActionResult<UserModel>> UpdateProfile(Guid id, [FromBody]UserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("user not found");
        }
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.NickName = dto.NickName;
        user.Password = dto.Password;
        return Ok(user);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("user not found");
        }
        await _userRepository.DeleteAsync(id);
        return Ok("user deleted");
    }
}
