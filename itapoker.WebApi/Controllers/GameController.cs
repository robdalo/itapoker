using AutoMapper;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;
using itapoker.SDK.Models;
using Microsoft.AspNetCore.Mvc;

using Domain = itapoker.Core.Domain;
using SDK = itapoker.SDK;

namespace itapoker.WebApi.Controllers;

[ApiController]
[Route("game")]
public class GameController : ControllerBase
{
    private readonly IGameEngine _gameEngine;
    private readonly IHighScoreRepo _highScoreRepo;
    private readonly IMapper _mapper;

    public GameController(
        IGameEngine gameEngine,
        IHighScoreRepo highScoreRepo,
        IMapper mapper)
    {
        _gameEngine = gameEngine;
        _highScoreRepo = highScoreRepo;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("anteup")]
    public IActionResult AnteUp(SDK.Requests.AnteUpRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.AnteUpRequest>(request);

        var response = _gameEngine.AnteUp(mapped);

        return Ok(_mapper.Map<SDK.Responses.AnteUpResponse>(response));
    }

    [HttpPost]
    [Route("deal")]
    public IActionResult Deal(SDK.Requests.DealRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.DealRequest>(request);

        var response = _gameEngine.Deal(mapped);

        return Ok(_mapper.Map<SDK.Responses.DealResponse>(response));
    }

    [HttpGet]
    [Route("highscores")]
    public IActionResult Get()
    {
        return Ok(_mapper.Map<List<HighScore>>(_highScoreRepo.Get()));
    }

    [HttpPost]
    [Route("singleplayer")]
    public IActionResult SinglePlayer(SDK.Requests.SinglePlayerRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.SinglePlayerRequest>(request);

        var response = _gameEngine.SinglePlayer(mapped);

        return Ok(_mapper.Map<SDK.Responses.SinglePlayerResponse>(response));
    }
}