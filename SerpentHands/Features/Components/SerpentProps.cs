using SerpentHands.Components;
using SerpentHands.Features.Components.Interfaces;

namespace SerpentHands.Features.Components;

public class SerpentProps(SerpentHandsProperties instance) : IPlayerPropertyModule
{
    public SerpentHandsProperties Instance { get; } = instance;
    public SerpentRole? SerpentRole { get; set; }
}