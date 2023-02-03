using System.Diagnostics.CodeAnalysis;
using ManxAudioSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManxAudioSearch.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController
{
    private readonly AudioService _audioService;

    public StatisticsController(AudioService audioService)
    {
        _audioService = audioService;
    }

    [HttpGet]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public Statistics Get() => new(_audioService.GetWords().Count(), _audioService.Files.Count);
}

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record Statistics(long NumberOfDistinctWords, long NumberOfFiles);