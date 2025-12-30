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
    public class Initiator : SerpentHandsRole
    {
        public override uint Id { get; set; } = 3;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Зачинщик отряда 'Длань Змея'";
        public override string Description { get; set; } = "<size=40><color=#34ebd2>Теперь вы <b></color>\n<color=#8A0014>Зачинщик отряда</color></b>.\n<b><color=#E63C53>Вы рождены для боя, а бой рождён для вас!</color></b></size>";
        public override SerpentRole SerpentRole { get; set; } = SerpentRole.Initiator;

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

                
                ev.Player.SerpentHandsProperties().SerpentProps.SerpentRole = SerpentRole.Initiator;
        
                ev.Player.MaxHumeShield = 25;
                ev.Player.HumeShield = 25;

                ev.Player.AddItem(ItemType.GunA7);
                ev.Player.AddItem(ItemType.GunCom45);
                CustomItem.TryGive(ev.Player, 5); // SHInitiatorKeycard
                ev.Player.AddItem(ItemType.SCP500);
                ev.Player.AddItem(ItemType.Medkit);
                ev.Player.AddItem(ItemType.Radio);
                ev.Player.AddItem(ItemType.ArmorHeavy);
                ev.Player.AddAmmo(AmmoType.Nato762, 160);
                ev.Player.AddAmmo(AmmoType.Nato9, 120);
                
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