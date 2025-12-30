using System;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using SerpentHands.Features.Components;

namespace SerpentHands.Components
{
    public abstract class SerpentHandsRole : CustomRole
    {
        public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
        public override string CustomInfo { get; set; } = String.Empty;
        public abstract SerpentRole SerpentRole { get; set; }
        /*public abstract ushort HumeShield { get; set; }
        public abstract ushort ArtificialHealth { get; set; }*/
    }
}