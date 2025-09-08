using AutoMapper;
using itapoker.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Domain = itapoker.Core.Domain;
using SDK = itapoker.SDK;

namespace itapoker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameEngine _gameEngine;
    private readonly IMapper _mapper;

    public GameController(IGameEngine gameEngine, IMapper mapper)
    {
        _gameEngine = gameEngine;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("singleplayer")]
    public IActionResult SinglePlayer(SDK.Requests.NewGameRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.NewGameRequest>(request);

        var response = _gameEngine.NewGame(mapped);

        return Ok(_mapper.Map<SDK.Responses.NewGameResponse>(response));
    }
}