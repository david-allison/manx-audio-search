using System.Reflection;
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
            .Concat(isManx ? Utils.GetManxAlternates(query) : Array.Empty<string>())
            .Concat(isEnglish ? _translationService.ToManx(query) : Array.Empty<string>())
            .Distinct();


        var results = manxWords
            .ToLookup(x => Utils.NormaliseAlternate(x, query), x => _audioService.GetFilesContainingWord(x))
            .ToDictionary(x => x.Key, x => x.SelectMany(u => u).ToList())
            .Select(x => new SearchResult(x.Key,
                _translationService.ToEnglish(x.Key).OrderByDescending(x => x.Length).ToArray(), // order by length to get the closest to the search first
                x.Value.Select(z => new AudioFile(z.FileNameNoExtension, z.Transcription))
                    .OrderBy(x => x.Transcription.Length).ToArray()))
            .ToList();

        // if we have results, don't send "river" (0 results) to the client
        if (results.SelectMany(x => x.Files).Any())
        {
            results = results.Where(x => x.Files.Length > 0).ToList();
        }
                
        return new SearchResultList(results);
    }
}

public record AudioFile(string FileName, string Transcription);
public record SearchResult(string Word, string[] Translations, AudioFile[] Files);
public record SearchResultList(List<SearchResult> Results);