using AutoMapper;
using DataAccess.Models;
using Domain.Dtos;
using Message.API.Services;
using Message.DataAccess.Models;
using Message.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController(ILogger<MessageController> logger, IUnitOfWork unitOfWork, IMapper mapper, IPublishService publishService) : ControllerBase
{
    private readonly ILogger<MessageController> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IPublishService _publishService = publishService;

    [HttpGet("get/{id}")]
    public async Task<ActionResult<MessageDto>> GetMessage(Guid id)
    {
        var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
        if (message is null)
        {
            return NotFound("message not found");
        }
        var messageDto = _mapper.Map<MessageDto>(message);
        return Ok(messageDto);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<MessageModel>>> GetAllMessages()
    {
        var messages = await _unitOfWork.MessageRepository.GetAllAsync();
        if (messages is null)
        {
            return Ok(Enumerable.Empty<MessageModel>());
        }
        return Ok(messages);
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddMessage([FromBody]MessageDto dto)
    {
        try
        {
            // Create and save the message to the database
            var message = _mapper.Map<MessageModel>(dto);
            await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.SaveAsync();

            // Connect to MQTT broker and publish the message
            await _publishService.PublishAsync(dto);

            // Create the corresponding UserMessage entry
            var userMessage = new UserMessage
            {
                UserId = message.Sender,
                MessageId = message.Id
            };
            await _unitOfWork.UserMessageRepository.AddAsync(userMessage);
            await _unitOfWork.SaveAsync();

            return Ok("message created and published");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending the message and adding UserMessage");
            return BadRequest("An error occurred while sending the message and adding UserMessage");
        }
    }

    [HttpPut("edit/{id}")]
    public async Task<ActionResult<MessageModel>> EditMessage(Guid id, [FromBody] string content)
    {
        var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
        if(message is null)
        {
            return NotFound("message is not found");
        }
        message.Content = content;
        await _unitOfWork.MessageRepository.UpdateAsync(message);
        await _unitOfWork.SaveAsync();
        return Ok(message);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> RemoveMessage(Guid id)
    {
        var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
        if (message is null)
        {
            return NotFound("message is not found");
        }
        await _unitOfWork.MessageRepository.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
        return Ok("message deleted");
    }
}
