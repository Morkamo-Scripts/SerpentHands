using Exiled.API.Features;
using SerpentHands.Features.Components;
using UnityEngine;

namespace SerpentHands.Components;

public sealed class SerpentHandsProperties() : MonoBehaviour
{
    private void Awake()
    {
        Player = Player.Get(gameObject);
        SerpentProps = new SerpentProps(this);
    }

    public Player Player { get; private set; }
    public SerpentProps SerpentProps { get; private set; }
}