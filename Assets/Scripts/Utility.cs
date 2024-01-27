using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class Utility
{
    public static string RemoveSpaces(string str)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != ' ')
            {
                builder.Append(str[i]);
            }
        }
        return builder.ToString();
    }
}
