using ManxAudioSearch.Controllers;
using ManxAudioSearch.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ManxAudioSearch.Tests;

[TestFixture]
public class SearchControllerTests
{
    private readonly SearchController _searchController;

    public SearchControllerTests()
    {
        using WebApplicationFactory<Program> factory = new();
        // TODO: Better DI
        _searchController = new SearchController(
            factory.Services.GetRequiredService<TranslationService>(),
            factory.Services.GetRequiredService<AudioService>()
        );
    }

    [Test]
    public void River_Test()
    {
        var results = _searchController.Search("river", true, true);
        
        Assert.That(results.Results.Select(x => x.Word), Is.EquivalentTo(new [] { "awin", "strooan" }));
    }
    
    [Test]
    public void Police_Test()
    {
        var results = _searchController.Search("meoir-shee", true, true);
        
        Assert.That(results.Results.Select(x => x.Word), Is.EquivalentTo(new [] { "meoir-shee" }));
        
        var fileNames = results.Results.Single(x => x.Word == "meoir-shee").Files.Select(x => x.FileName);

        // but, we contain a word which is 'meoirshee2', which is transcribed as 'meoirshee'
        Assert.That(fileNames, Does.Contain("meoirshee2"));
    }
}