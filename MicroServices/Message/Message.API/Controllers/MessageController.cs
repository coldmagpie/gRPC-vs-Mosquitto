using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController(ILogger<MessageController> logger, IMessageRepository messageRepository, IMapper mapper) : ControllerBase
{
    private readonly ILogger<MessageController> _logger = logger;
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet("get/{id}")]
    public async Task<ActionResult<MessageDto>> GetMessage(Guid id)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        if (message is null)
        {
            return NotFound("message not found");
        }
        var messageDto = _mapper.Map<MessageDto>(message);
        return Ok(messageDto);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<Message>>> GetAllMessages()
    {
        var messages = await _messageRepository.GetAllAsync();
        if (messages is null)
        {
            return Ok(Enumerable.Empty<Message>());
        }
        return Ok(messages);
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddMessage([FromBody]MessageDto dto)
    {
        var message = _mapper.Map<Message>(dto);
        await _messageRepository.AddAsync(message);
        return Ok("message created");
    }

    [HttpPut("edit/{id}")]
    public async Task<ActionResult<Message>> EditMessage(Guid id, [FromBody] string content)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        if(message is null)
        {
            return NotFound("message is not found");
        }
        message.Content = content;
        await _messageRepository.UpdateAsync(message);
        return Ok(message);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<Message>> RemoveMessage(Guid id)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        if (message is null)
        {
            return NotFound("message is not found");
        }
        await _messageRepository.DeleteAsync(id);
        return Ok("message deleted");
    }
}
