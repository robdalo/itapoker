using AutoMapper;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;
using itapoker.Core.Services.Interfaces;
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
    private readonly ISecurityService _securityService;
    private readonly IValidationService _validationService;

    public GameController(
        IGameEngine gameEngine,
        IHighScoreRepo highScoreRepo,
        IMapper mapper,
        ISecurityService securityService,
        IValidationService validationService)
    {
        _gameEngine = gameEngine;
        _highScoreRepo = highScoreRepo;
        _mapper = mapper;
        _securityService = securityService;
        _validationService = validationService;
    }

    [HttpPost]
    [Route("chip/add")]
    public IActionResult AddChip(SDK.Requests.AddChipRequest request)
    {
        try
        {
            var mapped = _mapper.Map<Domain.Requests.AddChipRequest>(request);

            _validationService.AddChip(mapped);

            var game = _gameEngine.AddChip(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("anteup")]
    public IActionResult AnteUp(SDK.Requests.AnteUpRequest request)
    {
        try
        {
            var mapped = _mapper.Map<Domain.Requests.AnteUpRequest>(request);

            _validationService.AnteUp(mapped);

            var game = _gameEngine.AnteUp(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("bet")]
    public IActionResult Bet(SDK.Requests.BetRequest request)
    {
        try
        {
            var mapped = _mapper.Map<Domain.Requests.BetRequest>(request);

            _validationService.Bet(mapped);

            var game = _gameEngine.Bet(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("deal")]
    public IActionResult Deal(SDK.Requests.DealRequest request)
    {
        try
        {
            var mapped = _mapper.Map<Domain.Requests.DealRequest>(request);

            _validationService.Deal(mapped);

            var game = _gameEngine.Deal(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("draw")]
    public IActionResult Draw(SDK.Requests.DrawRequest request)
    {
        try
        {
            var mapped = _mapper.Map<Domain.Requests.DrawRequest>(request);

            _validationService.Draw(mapped);

            var game = _gameEngine.Draw(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("hold")]
    public IActionResult Hold(SDK.Requests.HoldRequest request)
    {
        try
        {
            var mapped = _mapper.Map<Domain.Requests.HoldRequest>(request);

            _validationService.Hold(mapped);

            var game = _gameEngine.Hold(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
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
        try
        {
            var mapped = _mapper.Map<Domain.Requests.NextRequest>(request);

            _validationService.Next(mapped);

            var game = _gameEngine.Next(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("chip/remove")]
    public IActionResult RemoveChip(SDK.Requests.RemoveChipRequest request)
    {
        try
        {
            var mapped = _mapper.Map<Domain.Requests.RemoveChipRequest>(request);

            _validationService.RemoveChip(mapped);

            var game = _gameEngine.RemoveChip(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
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
        try
        {
            var mapped = _mapper.Map<Domain.Requests.ShowdownRequest>(request);

            _validationService.Showdown(mapped);

            var game = _gameEngine.Showdown(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("singleplayer")]
    public IActionResult SinglePlayer(SDK.Requests.SinglePlayerRequest request)
    {
        try
        {
            var mapped = _mapper.Map<Domain.Requests.SinglePlayerRequest>(request);

            _validationService.SinglePlayer(mapped);

            var game = _gameEngine.SinglePlayer(mapped);

            _securityService.Redact(game);

            return Ok(_mapper.Map<Game>(game));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}