using System.Diagnostics.CodeAnalysis;
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
        Phrase ToPhrase(Services.AudioFile file) => new(file.FileNameNoExtension, file.Transcription);
        var words = _audioService
            .Words
            .Select(word => 
                new
                {
                    Word = word, 
                    Phrases = _audioService.GetFilesContainingWord(word).Select(ToPhrase)
                }).ToList();
        return words;
    }
}

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
internal record Phrase(string FileName, string Transcription);