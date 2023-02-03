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
}