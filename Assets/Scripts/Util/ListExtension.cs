using System;
using System.Collections.Generic;

public static class ListExtension
{
    private static Random random = new Random();

    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
    }
}
