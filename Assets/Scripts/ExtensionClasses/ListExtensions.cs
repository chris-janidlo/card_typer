using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static string ToNaturalString<T> (this List<T> list)
    {
        string output = list[0].ToString();

        if (list.Count == 1) return output;

        if (list.Count == 2) return $"{output} and {list[1]}";

        for (int i = 1; i < list.Count - 1; i++)
        {
            output += $", {list[i]}";
        }
        
        return $"{output}, and {list[list.Count - 1]}";
    }
}
