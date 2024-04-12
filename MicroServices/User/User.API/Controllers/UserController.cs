using AutoMapper;
using DataAccess.Models;
using DataAccess.UnitOfWork;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(ILogger<UserController>logger, IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    [HttpPost("add")]
    public async Task<ActionResult> Register([FromBody]UserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveAsync();
        return Ok("user created");
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("user not found");
        }
        var userDto = _mapper.Map<UserDto>(user);
        return Ok(userDto);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        if (users is null)
        {
            return Ok(new List<User>());
        }
        return Ok(users);
    }

    [HttpPut("update")]
    public async Task<ActionResult<User>> UpdateProfile(Guid id, [FromBody]UserDto dto)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("user not found");
        }
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.NickName = dto.NickName;
        user.Password = dto.Password;
        await _unitOfWork.SaveAsync();
        return Ok(user);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("user not found");
        }
        await _unitOfWork.UserRepository.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
        return Ok("user deleted");
    }
}
