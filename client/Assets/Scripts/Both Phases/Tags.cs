using System;

[Serializable]
public struct TagPair
{
    public string Start, End;

    public string Wrap (string target)
    {
        return Start + target + End;
    }
    
    public string Wrap (char target)
    {
        return Start + target + End;
    }
}

[Serializable]
public struct LinkTag
{
    public string ID;

    public string Wrap (string target, int index)
    {
        return $"<link=\"{ID}:{index}\">{target}</link>";
    }
}
