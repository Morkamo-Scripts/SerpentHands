using System;
using Exiled.API.Features;
using Exiled.CustomItems.API;
using Exiled.CustomRoles.API;
using HarmonyLib;
using SerpentHands.Handlers;

namespace SerpentHands
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => nameof(SerpentHands);
        public override string Prefix => Name;
        public override string Author => "Morkamo";
        public override Version Version => new Version(1, 1, 0);
        public override Version RequiredExiledVersion => new Version(9, 12, 1);

        public static Plugin Instance { get; private set; }
        private static Harmony _harmony;
        
        private GeneralSettings _generalSettings;
        /*private VoiceChatHandler _voiceChatHandler;*/
        public RoundHandler RoundHandler;

        public override void OnEnabled()
        {
            Instance = this;
            _harmony = new Harmony("ru.morkamo.serpentHands.patches");
            _generalSettings = Config.GeneralSettings;
            RoundHandler = new RoundHandler();
            /*_voiceChatHandler = new VoiceChatHandler();*/
            
            MorkamoEventsRegistrator.Plugin.AddRegistrator(RoundHandler);
            /*MorkamoEventsRegistrator.Plugin.AddRegistrator(_voiceChatHandler);*/
            
            Config.SquadRoles.Leader.Register();
            Config.SquadRoles.Eagle.Register();
            Config.SquadRoles.Initiator.Register();
            Config.SquadRoles.Jumper.Register();
            Config.SquadRoles.Support.Register();
            
            Config.SquadKeycards.KeycardLeader.Register();
            Config.SquadKeycards.KeycardEagle.Register();
            Config.SquadKeycards.KeycardInitiator.Register();
            Config.SquadKeycards.KeycardJumper.Register();
            Config.SquadKeycards.KeycardSupport.Register();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Config.SquadKeycards.KeycardLeader.Unregister();
            Config.SquadKeycards.KeycardEagle.Unregister();
            Config.SquadKeycards.KeycardInitiator.Unregister();
            Config.SquadKeycards.KeycardJumper.Unregister();
            Config.SquadKeycards.KeycardSupport.Unregister();
            
            Config.SquadRoles.Leader.Unregister();
            Config.SquadRoles.Eagle.Unregister();
            Config.SquadRoles.Initiator.Unregister();
            Config.SquadRoles.Jumper.Unregister();
            Config.SquadRoles.Support.Unregister();
            
            /*MorkamoEventsRegistrator.Plugin.RemoveRegistrator(_voiceChatHandler);*/
            MorkamoEventsRegistrator.Plugin.RemoveRegistrator(RoundHandler);
            
            RoundHandler = null;
            _generalSettings = null;
            _harmony = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}