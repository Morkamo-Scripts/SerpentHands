using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using MorkamoEventsRegistrator.Components;
using RueI.API;
using RueI.API.Elements;
using SerpentHands.Components;
using SerpentHands.Events;
using SerpentHands.Extensions;
using SerpentHands.Features.Components;
using events = Exiled.Events.Handlers;

namespace SerpentHands.Roles
{
    public class Leader : SerpentHandsRole
    {
        public override uint Id { get; set; } = 1;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Лидер отряда 'Длань Змея'";
        public override string Description { get; set; } = "<size=40><color=#34ebd2>Теперь вы</color>\n<b><color=#FF1A57><b>Лидер отряда</color></b>.\n<b><color=#FA618B>Ведите свою команду к победе!</b></color></b></size>";
        public override SerpentRole SerpentRole { get; set; } = SerpentRole.Leader;

        protected override void SubscribeEvents()
        {
            events.Player.Spawned += OnPlayerSpawned;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            events.Player.Spawned -= OnPlayerSpawned;
            base.UnsubscribeEvents();
        }

        private void OnPlayerSpawned(SpawnedEventArgs ev)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                if (!Check(ev.Player))
                    return;
            
                ev.Player.SerpentHandsProperties().SerpentProps.SerpentRole = SerpentRole.Leader;
            
                ev.Player.MaxHumeShield = 50;
                ev.Player.HumeShield = 50;

                ev.Player.AddItem(ItemType.GunE11SR);
                ev.Player.AddItem(ItemType.GunCOM18);
                CustomItem.TryGive(ev.Player, 3); // SHLeaderKeycard
                ev.Player.AddItem(ItemType.SCP500);
                ev.Player.AddItem(ItemType.Radio);
                ev.Player.AddItem(ItemType.ArmorCombat);
                ev.Player.AddItem(ItemType.GrenadeFlash);
                ev.Player.AddAmmo(AmmoType.Nato556, 90);
                ev.Player.AddAmmo(AmmoType.Nato9, 24);
                
                RueDisplay.Get(ev.Player).Show(
                    new Tag(),
                    new BasicElement(200, Description), 5);

                Timing.CallDelayed(5.1f, () => RueDisplay.Get(ev.Player).Update());
            });
        }

        protected override void RoleRemoved(Player player)
        {
            base.RoleRemoved(player);
            player?.SerpentHandsProperties()?.SerpentProps.SerpentRole = null;
        }
    }
}