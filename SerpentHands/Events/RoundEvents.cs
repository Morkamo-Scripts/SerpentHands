using System;
using System.Collections.Generic;
using Exiled.API.Features;
using SerpentHands.Events.EventArgs.Round;

namespace SerpentHands.Events;

public partial class RoundEvents
{
    public event Action<SerpentsHandRespawnedEventArgs> SerpentsHandRespawned;
}

public partial class RoundEvents
{
    public void InvokeSerpentsHandRespawned(List<Player> squad)
    {
        var ev = new SerpentsHandRespawnedEventArgs(squad);
        SerpentsHandRespawned?.Invoke(ev);
    }
}