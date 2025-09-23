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
    [Route("chip/add")]
    public IActionResult AddChip(SDK.Requests.AddChipRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.AddChipRequest>(request);

        var game = _gameEngine.AddChip(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpPost]
    [Route("anteup")]
    public IActionResult AnteUp(SDK.Requests.AnteUpRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.AnteUpRequest>(request);

        var game = _gameEngine.AnteUp(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpPost]
    [Route("bet")]
    public IActionResult Bet(SDK.Requests.BetRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.BetRequest>(request);

        var game = _gameEngine.Bet(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpPost]
    [Route("deal")]
    public IActionResult Deal(SDK.Requests.DealRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.DealRequest>(request);

        var game = _gameEngine.Deal(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpPost]
    [Route("draw")]
    public IActionResult Draw(SDK.Requests.DrawRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.DrawRequest>(request);

        var game = _gameEngine.Draw(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpPost]
    [Route("hold")]
    public IActionResult Hold(SDK.Requests.HoldRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.HoldRequest>(request);

        var game = _gameEngine.Hold(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpGet]
    [Route("highscores")]
    public IActionResult HighScores()
    {
        return Ok(_mapper.Map<List<HighScore>>(_highScoreRepo.Get()));
    }

    [HttpPost]
    [Route("next")]
    public IActionResult Next(SDK.Requests.NextRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.NextRequest>(request);

        var game = _gameEngine.Next(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpPost]
    [Route("chip/remove")]
    public IActionResult RemoveChip(SDK.Requests.RemoveChipRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.RemoveChipRequest>(request);

        var game = _gameEngine.RemoveChip(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpPost]
    [Route("decision")]
    public IActionResult SetDecision(SDK.Requests.SetDecisionRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.SetDecisionRequest>(request);

        var game = _gameEngine.SetDecision(mapped);

        return Ok(_mapper.Map<Game>(game));
    }

    [HttpPost]
    [Route("showdown")]
    public IActionResult Showdown(SDK.Requests.ShowdownRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.ShowdownRequest>(request);

        var game = _gameEngine.Showdown(mapped);

        return Ok(_mapper.Map<Game>(game));
    }
    
    [HttpPost]
    [Route("singleplayer")]
    public IActionResult SinglePlayer(SDK.Requests.SinglePlayerRequest request)
    {
        var mapped = _mapper.Map<Domain.Requests.SinglePlayerRequest>(request);

        var game = _gameEngine.SinglePlayer(mapped);

        return Ok(_mapper.Map<Game>(game));
    }
}