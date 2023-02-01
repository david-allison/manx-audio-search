using ManxAudioSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManxAudioSearch.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly AudioService _audioService;

    public WeatherForecastController(AudioService audioService)
    {
        _audioService = audioService;
    }

    [HttpGet]
    public IEnumerable<object> Get()
    {
        var words = _audioService.GetWords().Select(x => new { Word = x }).ToList();
        Console.WriteLine(words.Count);
        return words;
    }
}