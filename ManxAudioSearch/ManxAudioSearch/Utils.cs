namespace ManxAudioSearch;

public class Utils
{
    public static string GetLocalFile(params string[] inputPath)
    {
        String[] path = new string[inputPath.Length + 1];
        path[0] = AppDomain.CurrentDomain.BaseDirectory;
        for (int i = 0; i < inputPath.Length; i++)
        {
            path[i + 1] = inputPath[i];
        }

        return Path.Combine(path);
    }
    
    public static IEnumerable<string> GetManxAlternates(string query)
    {
        yield return query; // "meoir shee": definition, but no results
        if (query.Contains(' '))
        {
            yield return query.Replace(" ", "-"); // "meoir-shee": no definition, but many results
            yield return query.Replace(" ", "");  // "meoirshee":  no definition, 1 result
        }
        if (query.Contains($"-"))
        {
            yield return query.Replace("-", " "); //
            yield return query.Replace("-", "");  // 
        }
    }
}