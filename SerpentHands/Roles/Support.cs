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