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
    public void test()
    {
        _searchController.Search("true", true, true);
    }
}