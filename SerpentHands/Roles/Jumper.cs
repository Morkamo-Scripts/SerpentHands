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
using UnityEngine;
using events = Exiled.Events.Handlers;

namespace SerpentHands.Roles
{
    public class Jumper : SerpentHandsRole
    {
        public override uint Id { get; set; } = 4;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Джампер отряда 'Длань Змея'";
        public override string Description { get; set; } = "<size=40><color=#34ebd2>Теперь вы </color>\n<b><color=#FA399A>Джампер отряда</color></b>.\n<b><color=#FA6BB2>Трудно в бою, легко на ветру!</color></b></size>";
        public override SerpentRole SerpentRole { get; set; } = SerpentRole.Jumper;

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
            
                Round.IgnoredPlayers.Add(ev.Player.ReferenceHub);
                
                ev.Player.SerpentHandsProperties().SerpentProps.SerpentRole = SerpentRole.Jumper;
        
                ev.Player.MaxHumeShield = 25;
                ev.Player.HumeShield = 25;

                ev.Player.AddItem(ItemType.GunShotgun);
                ev.Player.AddItem(ItemType.Jailbird);
                CustomItem.TryGive(ev.Player, 6); // SHJumperKeycard
                ev.Player.AddItem(ItemType.Medkit);
                ev.Player.AddItem(ItemType.Painkillers);
                ev.Player.AddItem(ItemType.SCP207);
                ev.Player.AddItem(ItemType.Radio);
                ev.Player.AddItem(ItemType.ArmorCombat);
                ev.Player.AddAmmo(AmmoType.Ammo12Gauge, 54);
                
                RueDisplay.Get(ev.Player).Show(
                    new Tag(),
                    new BasicElement(200, Description), 5);
                
                RueDisplay.Get(ev.Player).Show(
                    new Tag(),
                    new BasicElement(900, "<size=45><b><color=#34ebd2>Длань Змея и SCP являются союзными\nклассами и не могут наносить\nдруг другу урон!</color></b></size>"), 10);

                ev.Player.Rotation = new Quaternion(0, 0.7f, 0, 0.7f);
                
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