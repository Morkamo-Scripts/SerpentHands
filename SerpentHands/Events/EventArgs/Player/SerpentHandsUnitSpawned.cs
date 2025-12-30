using SerpentHands.Features.Components;

namespace SerpentHands.Events.EventArgs.Player
{
    using Exiled.API.Features;
    
    public class SerpentHandsUnitSpawned(Player player, SerpentRole serpentRole)
    {
        public Player Player => player;
        public SerpentRole SerpentRole => serpentRole;
    }
}