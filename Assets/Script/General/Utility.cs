using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static string TimeToString(float time)
    {
        int minutes = (int)(time / 60);
        float seconds = time % 60f;

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    // get next element in Enum
    // https://stackoverflow.com/questions/642542/how-to-get-next-or-previous-enum-value-in-c-sharp
    public static T Next<T>(this T src) where T : Enum
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }
}
