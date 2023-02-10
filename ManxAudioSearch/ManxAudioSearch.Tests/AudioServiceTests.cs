using ManxAudioSearch.Services;

namespace ManxAudioSearch.Tests;

[TestFixture]
public class AudioServiceTests
{
    [Test]
    public void SearchTwoWords()
    {
        var service = new AudioService(new List<AudioFile>
        {
            new()
            {
                Transcription = "hello world",
            }
        });

        var results = service.GetFilesContainingWord("hello world");
        
        Assert.That(results, Has.Count.EqualTo(1));
        var badResults = service.GetFilesContainingWord("hello warld");
        Assert.That(results, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void SearchNotFound()
    {
        var service = new AudioService(new List<AudioFile>
        {
            new()
            {
                Transcription = "hello",
            }
        });

        var results = service.GetFilesContainingWord("hello world");
        Assert.That(results, Has.Count.EqualTo(0));
        
        
        var noMatch = service.GetFilesContainingWord("world");
        Assert.That(noMatch, Has.Count.EqualTo(0));
    }

}