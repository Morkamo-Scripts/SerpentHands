using System;
using Exiled.API.Features;
using SerpentHands.Events.EventArgs.Player;
using SerpentHands.Features.Components;

namespace SerpentHands.Events
{
    public partial class PlayerEvents
    {
        public event Action<PlayerFullConnectedEventArgs> PlayerFullConnected;
        public event Action<SerpentHandsUnitSpawned> SerpentUnitSpawned;
    }

    public partial class PlayerEvents
    {
        public void InvokePlayerFullConnected(Player player)
        {
            var ev = new PlayerFullConnectedEventArgs(player);
            PlayerFullConnected?.Invoke(ev);
        }
        
        public void InvokeSerpentUnitSpawned(Player player, SerpentRole serpentRole)
        {
            var ev = new SerpentHandsUnitSpawned(player, serpentRole);
            SerpentUnitSpawned?.Invoke(ev);
        }
    }
}