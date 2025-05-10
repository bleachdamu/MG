using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Shuffle the list of items.
    /// </summary>
    public static void Shuffle<T>(this IList<T> itemList)
    {
        var count = itemList.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var rnd = Random.Range(i, count);
            var tmp = itemList[i];
            itemList[i] = itemList[rnd];
            itemList[rnd] = tmp;
        }
    }
}
