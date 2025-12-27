using System;
using Exiled.API.Features;
using HarmonyLib;

namespace SerpentHands
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => nameof(SerpentHands);
        public override string Prefix => Name;
        public override string Author => "Morkamo";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 12, 1);

        public static Plugin Instance { get; private set; }
        private static Harmony _harmony;

        public override void OnEnabled()
        {
            Instance = this;
            _harmony = new Harmony("ru.morkamo.serpentHands.patches");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _harmony = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}