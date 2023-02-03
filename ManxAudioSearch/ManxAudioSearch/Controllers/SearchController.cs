using ManxAudioSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManxAudioSearch.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController
{
    private readonly TranslationService _translationService;
    private readonly AudioService _audioService;

    public SearchController(TranslationService translationService, AudioService audioService)
    {
        _translationService = translationService;
        _audioService = audioService;
    }
    
    [HttpGet("{query}")]
    public SearchResultList Search(string query, [FromQuery(Name = "manx")] bool isManx, [FromQuery(Name = "english")] bool isEnglish)
    {
        if (!isEnglish && !isManx)
        {
            throw new ArgumentException("must search either English or Manx");
        }

        var manxWords = Array.Empty<string>()
            .Concat(isManx ? new[] { query } : Array.Empty<string>())
            .Concat(isEnglish ? _translationService.ToManx(query) : Array.Empty<string>());


        var results = manxWords.Select(x =>
            new {
                Word = x,
                Files = _audioService.GetFilesContainingWord(x)
            })
            .SelectMany(result => result.Files.Select(x => new SearchResult(result.Word, x.FileNameNoExtension, x.Transcription)))
            .ToList();
        return new SearchResultList(results);
    }
}

public record SearchResult(string Word, string FileName, string Transcription);
public record SearchResultList(List<SearchResult> Results);