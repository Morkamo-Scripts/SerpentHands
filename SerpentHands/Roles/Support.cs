using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RueI.API;
using RueI.API.Elements;
using SerpentHands.Components;
using SerpentHands.Extensions;
using SerpentHands.Features.Components;
using UnityEngine;
using events = Exiled.Events.Handlers;

namespace SerpentHands.Roles
{
    public class Support : SerpentHandsRole
    {
        public override uint Id { get; set; } = 5;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Поддержка отряда 'Длань Змея'";
        public override string Description { get; set; } = "<size=40><color=#34ebd2>Теперь вы </color>\n<b><color=#C91CFC>Поддержка отряда</color></b>.\n<b><color=#D96BFA>Твоя помощь обязательно понадобиться!</color></b></size>";
        public override SerpentRole SerpentRole { get; set; } = SerpentRole.Support;

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
                
                ev.Player.SerpentHandsProperties().SerpentProps.SerpentRole = SerpentRole.Support;
        
                ev.Player.AddItem(ItemType.GunCrossvec);
                CustomItem.TryGive(ev.Player, 7); // SHSupportKeycard
                ev.Player.AddItem(ItemType.Medkit);
                ev.Player.AddItem(ItemType.Painkillers);
                ev.Player.AddItem(ItemType.Radio);
                ev.Player.AddItem(ItemType.ArmorCombat);
                ev.Player.AddAmmo(AmmoType.Nato9, 90);
                
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