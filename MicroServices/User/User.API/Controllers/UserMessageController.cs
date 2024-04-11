using AutoMapper;
using DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class UserMessageController(ILogger<UserMessageController> logger, IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly ILogger<UserMessageController> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
}

