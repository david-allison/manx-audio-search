using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ManxAudioSearch.Services;

public class AudioService
{
    private readonly List<AudioFile> _audioFiles;

    private AudioService(List<AudioFile> audioFiles)
    {
        _audioFiles = audioFiles;
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public static AudioService CreateInstance()
    {
        // requires `dotnet publish`
        var audioFilesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "publish", "wwwroot", "audio");
        
        var audioPaths = Directory.GetFiles(audioFilesDir, "*.mp3", SearchOption.AllDirectories).ToList();

        var metadataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AudioData");
        
        // TODO: This doesn't support folders yet
        var metadataPaths = audioPaths.ToDictionary(x => x, x => Path.Combine(metadataDir, Path.GetFileName(x) + ".json"));

        List<(string filePath, AudioJson metadata)> validPaths = audioPaths.Where(File.Exists).OrderBy(x => x).Select(filePath =>
        {
            var metadataPath = metadataPaths[filePath];
            string jsonString = File.ReadAllText(metadataPath);
            AudioJson metadata = JsonSerializer.Deserialize<AudioJson>(jsonString, JsonSerializerOptions)!;
            return (filePath, metadata);

        }).ToList();

        if (metadataPaths.Count != validPaths.Count)
        {
            Console.WriteLine(metadataPaths.Count - validPaths.Count + " invalid paths");    
        }

        string[] ExtractWordsFromPath(string fle)
        {
            var title = Path.GetFileNameWithoutExtension(fle);
            var removeSuffix = Regex.Replace(title, @" \(\d+\)", "");
            var removeSuffixNoSpace = Regex.Replace(removeSuffix, @"([^\d])\d+", "$1");
            return new[] { removeSuffixNoSpace };
        }
        var files = validPaths.OrderByDescending(x => x).Select(x => new AudioFile
        {
            Title = x.filePath,
            Words = x.metadata.Transcription?.Split(" ") ?? ExtractWordsFromPath(x.filePath)
        }).ToList();

        // TODO: blah heh -> lukewarm, two words
        // jin nagh -> jinnagh
        // chammah
        // chanaikpeiagherbeeynmeaoirshee?
        // chanyhrys.
        // cremysh
        // cren => cre'n
        // deiney seyrey - not in dict
        // echooat
        // enmyssitjeeal
        // erash or er ash?
        return new AudioService(files);
    }

    public IEnumerable<string> GetWords()
    {
        // ensure we don't need to refresh the app while testing
        var audioFiles = CreateInstance()._audioFiles;

        return audioFiles.SelectMany(x => x.Words).Select(x => x.ToLowerInvariant()
            .Replace("?", "")) //As craad ta moirrey? 
            .Distinct().OrderBy(x => x);
    }
}

// non-null not initialised
#pragma warning disable CS8618 
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AudioFile
{
    public string Title { get; set; }
    public string[] Words { get; set; }
    public bool IsKnownData { get; set; }
}
#pragma warning restore CS8618

public class AudioJson
{
    public string? Transcription { get; set; }
}