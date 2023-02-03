using ManxAudioSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManxAudioSearch.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController
{
    private readonly TranslationService _translationService;

    public SearchController(TranslationService translationService)
    {
        _translationService = translationService;
    }
    
    [HttpGet("{query}")]
    public IEnumerable<string> Search(string query, [FromQuery(Name = "manx")] bool isManx, [FromQuery(Name = "english")] bool isEnglish)
    {
        if (!isEnglish && !isManx)
        {
            throw new ArgumentException("must search either English or Manx");
        }

        var manxWords = Array.Empty<string>()
            .Concat(isManx ? new[] { query } : Array.Empty<string>())
            .Concat(isEnglish ? _translationService.ToManx(query) : Array.Empty<string>());


        return manxWords;
    }
}

public record SearchResult();
public record SearchResultList(SearchResult[] Results);