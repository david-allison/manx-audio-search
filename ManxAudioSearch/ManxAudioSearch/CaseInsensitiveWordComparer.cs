namespace ManxAudioSearch;

internal class CaseInsensitiveWordComparer : IEqualityComparer<string>
{
    public static readonly CaseInsensitiveWordComparer Default = new CaseInsensitiveWordComparer();
    public bool Equals(string? x, string? y)
    {
        return string.Equals(x?.ToLowerInvariant(), y?.ToLowerInvariant());
    }

    public int GetHashCode(string obj)
    {
        return obj.ToLowerInvariant().GetHashCode();
    }
}