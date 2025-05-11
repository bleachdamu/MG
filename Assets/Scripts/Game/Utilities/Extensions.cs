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
        for (var i = 0; i < itemList.Count - 1; ++i)
        {
            var rnd = Random.Range(i, itemList.Count);
            var tmp = itemList[i];
            itemList[i] = itemList[rnd];
            itemList[rnd] = tmp;
        }
    }
}
