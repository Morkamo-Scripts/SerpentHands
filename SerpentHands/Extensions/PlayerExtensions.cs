using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using SerpentHands.Components;
using UnityEngine;

namespace SerpentHands.Extensions;

public static class PlayerExtensions
{
    public static SerpentHandsProperties SerpentHandsProperties(this Player player)
        => player.ReferenceHub.gameObject.GetComponent<SerpentHandsProperties>();
    
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}