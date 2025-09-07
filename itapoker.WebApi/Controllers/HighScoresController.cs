using AutoMapper;
using itapoker.Core.Repositories.Interfaces;
using itapoker.SDK.Models;
using Microsoft.AspNetCore.Mvc;

namespace itapoker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HighScoresController : ControllerBase
{
    private readonly IMapper _mapper;
    
    private readonly IHighScoreRepo _highScoreRepo;

    public HighScoresController(IHighScoreRepo highScoreRepo, IMapper mapper)
    {
        _highScoreRepo = highScoreRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_mapper.Map<List<HighScore>>(_highScoreRepo.Get()));
    }
}