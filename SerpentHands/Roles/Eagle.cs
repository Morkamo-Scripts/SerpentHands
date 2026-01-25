using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
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
    public class Eagle : SerpentHandsRole
    {
        public override uint Id { get; set; } = 2;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Орёл отряда 'Длань Змея'";
        public override string Description { get; set; } = "<size=40><color=#34ebd2>Теперь вы <b></color>\n<color=#FA396F>Орёл отряда</color></b>.\n<b><color=#FF6691>Вы правая рука лидера.</color>\n<color=#C70000><s>В вашем арсенале есть SCP-2158!</s></color></b></size>";
        public override SerpentRole SerpentRole { get; set; } = SerpentRole.Eagle;

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
            Timing.CallDelayed(1f, () =>
            {
                if (!Check(ev.Player))
                    return;
            
                Round.IgnoredPlayers.Add(ev.Player.ReferenceHub);
                
                ev.Player.SerpentHandsProperties().SerpentProps.SerpentRole = SerpentRole.Eagle;
        
                ev.Player.MaxHumeShield = 50;
                ev.Player.HumeShield = 50;

                ev.Player.AddItem(ItemType.GunE11SR);
                /*ev.Player.AddItem(ItemType.GunRevolver);*/
                CustomItem.TryGive(ev.Player, 8); // SCP-2158
                CustomItem.TryGive(ev.Player, 4); // SHEagleKeycard
                ev.Player.AddItem(ItemType.SCP500);
                ev.Player.AddItem(ItemType.Radio);
                ev.Player.AddItem(ItemType.ArmorCombat);
                ev.Player.AddAmmo(AmmoType.Nato556, 90);
                ev.Player.AddAmmo(AmmoType.Ammo44Cal, 36);
                
                RueDisplay.Get(ev.Player).Show(
                    new Tag(),
                    new BasicElement(250, Description), 10);

                RueDisplay.Get(ev.Player).Show(
                    new Tag(),
                    new BasicElement(900, "<size=45><b><color=#34ebd2>Длань Змея и SCP являются союзными\nклассами и не могут наносить\nдруг другу урон!</color></b></size>"), 15);
                
                ev.Player.Rotation = new Quaternion(0, 0.7f, 0, 0.7f);
                
                Timing.CallDelayed(10.1f, () => RueDisplay.Get(ev.Player).Update());
                Timing.CallDelayed(15.1f, () => RueDisplay.Get(ev.Player).Update());
            });
        }
        
        protected override void RoleRemoved(Player player)
        {
            base.RoleRemoved(player);
            player?.SerpentHandsProperties()?.SerpentProps.SerpentRole = null;
        }
    }
}