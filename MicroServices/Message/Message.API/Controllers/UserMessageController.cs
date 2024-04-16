using AutoMapper;
using Domain.Dtos;
using Message.API.Services;
using Message.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using MQTTnet.Client;

namespace Message.API.Controllers;

public class UserMessageController(ILogger<UserMessageController> logger, IUnitOfWork unitOfWork, IPublishService mosquittoService, IMapper mapper, IMqttClient client) : ControllerBase
{
    private readonly ILogger<UserMessageController> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPublishService _mosquittoService = mosquittoService;
    private readonly IMapper _mapper = mapper;
    private readonly IMqttClient _mqttClient = client;

    //[HttpPost("add")]
    //public async Task<ActionResult> CreateUserMessage([FromBody] MessageDto dto, Guid id)
    //{
    //    try
    //    {
    //        var message = dto;
    //        message.Sender = id;
    //        return Ok("user message added");
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while adding user message");
    //    }
    //    return BadRequest("failed to add user message");
    //}

    //[HttpPost("all")]
    //public async Task<ActionResult<IEnumerable<UserMessageDto>>> GetAllUserMessage()
    //{
    //    var userMessages = await _unitOfWork.UserMessageRepository.GetAllAsync();
    //    var userMessageDtos = _mapper.Map<IEnumerable<UserMessageDto>>(userMessages).ToList();
    //    if (userMessages is null)
    //    {
    //        return new List<UserMessageDto>();
    //    }
    //    return Ok(userMessageDtos);
    //}
}
