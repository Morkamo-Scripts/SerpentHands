using System.Collections.Generic;

namespace SerpentHands.Events.EventArgs.Round;
using Exiled.API.Features;

public class SerpentsHandRespawnedEventArgs(List<Player> players)
{
    public List<Player> Players { get; private set; } = players;
}