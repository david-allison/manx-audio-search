using static System.Text.Json.JsonSerializer;

namespace ManxAudioSearch.Services;

public class TranslationService
{
    private readonly Dictionary<string, IList<string>> _manxToEnglishDictionary;
    private readonly Dictionary<string, IList<string>> _englishToManxDictionary;

    private TranslationService(
        Dictionary<string,IList<string>> manxToEnglishDictionary, 
        Dictionary<string,IList<string>> englishToManxDictionary)
    {
        _manxToEnglishDictionary = manxToEnglishDictionary;
        _englishToManxDictionary = englishToManxDictionary;
    }

    internal static TranslationService CreateInstance()
    {
        Dictionary<string, IList<string>> ToCaseInsensitiveDict(FileStream fileStream) 
        {
            var dict = DeserializeAsync<Dictionary<string, IList<string>>>(fileStream).Result!;
            return new Dictionary<string, IList<string>>(dict, StringComparer.OrdinalIgnoreCase);
        }
        // This saves ~700MB RAM compared to using F# for XML reading... sorry
        // files sourced from Phil Kelly https://www.learnmanx.com/page_342285.html
        using FileStream manx = File.OpenRead(Utils.GetLocalFile("Resources", "phil-kelly-manx-dictionary-data", "manx.json"));
        using FileStream english = File.OpenRead(Utils.GetLocalFile("Resources", "phil-kelly-manx-dictionary-data", "english.json"));
            

        var manxToEnglishDictionary = ToCaseInsensitiveDict(manx);
        var englishToManxDictionary = ToCaseInsensitiveDict(english);
        return new TranslationService(manxToEnglishDictionary, englishToManxDictionary);
    }

    public IEnumerable<string> ToManx(string query)
    {
        return _englishToManxDictionary.GetValueOrDefault(query, new List<string>());
    }

    public IEnumerable<string> ToEnglish(string query)
    {
        return _manxToEnglishDictionary.GetValueOrDefault(query, new List<string>());
    }
}