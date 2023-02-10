using ManxAudioSearch.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ManxAudioSearch.Tests;

[TestFixture]
public class BasicTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Test]
    [Ignore("Not implemented")]
    public void Translation_Works()
    {
        var client = _factory.Services.GetService<TranslationService>()!;

        var tests = new[]
        {
            "meoir shee",
            "meoir-shee",
            // TODO
            // "meoirshee", // don't know where to place the space yet.
            // We want to go from "meoir shee" -> "meoirshee" which allows the mapping, rather than infinitely splitting the input string
        };

        foreach (var word in tests)
        {
            Assert.That(client.ToEnglish(word), Does.Contain("policeman"), $"'{word}' should be translated to policeman");
        }
    }
}