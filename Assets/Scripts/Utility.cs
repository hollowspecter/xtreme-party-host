using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static string PrintList(this List<KeyValuePair<float, AbstractAction>> list)
    {
        string result = "";
        foreach (var item in list)
        {
            result += string.Format("\nScore={0} Action={1}", item.Key, item.Value);
        }
        return result;
    }
}