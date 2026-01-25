/*using System.Collections.Generic;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Mirror;
using MorkamoEventsRegistrator.Components;
using SerpentHands.Events;
using SerpentHands.Events.EventArgs.Player;
using SerpentHands.Extensions;
using UnityEngine;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;

namespace SerpentHands.Handlers;

public class VoiceChatHandler : IEventsRegistrator
{
    public void RegisterEvents()
    {
        EventManager.PlayerEvents.PlayerFullConnected += OnPlayerFullconnected;
        Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
    }

    public void UnregisterEvents()
    {
        EventManager.PlayerEvents.PlayerFullConnected -= OnPlayerFullconnected;
        Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
    }

    private void OnPlayerFullconnected(PlayerFullConnectedEventArgs ev)
    {
        
    }
    
    private void OnVoiceChatting(VoiceChattingEventArgs ev)
    {
        if (ev.Player == null)
            return;

        if (ev.Player.IsNPC)
            return;

        if (ev.VoiceMessage.Channel != VoiceChatChannel.Proximity)
            return;

        foreach (var target in Player.List)
        {
            if (target == ev.Player)
                continue;

            if (!target.IsScp)
                continue;

            if (target.Role is not IVoiceRole voiceRole)
                continue;

            var canReceive = voiceRole.VoiceModule.ValidateReceive(ev.Player.ReferenceHub, VoiceChatChannel.ScpChat);

            if (canReceive == VoiceChatChannel.None)
            {
                Log.Info("1");
                continue;
            }

            var conn = target.ReferenceHub?.connectionToClient;

            if (conn == null)
                continue;

            var forwarded = new VoiceMessage
            {
                Speaker = ev.Player.ReferenceHub,
                Channel = VoiceChatChannel.ScpChat,
                Data = ev.VoiceMessage.Data,
                DataLength = ev.VoiceMessage.DataLength
            };

            conn.Send(forwarded);
            Log.Info("2");
        }
    }
    
    // THIS CLASS FROM ANOTHER PLUGIN - "SCP PROXIMITY CHAT"
    public class OpusHandler
    {
        private static readonly Dictionary<Player, OpusHandler> Handlers = new();

        public OpusDecoder Decoder { get; } = new();
        public OpusEncoder Encoder { get; } = new(OpusApplicationType.Voip);

        public static OpusHandler Get(Player player)
        {
            if (Handlers.TryGetValue(player, out OpusHandler opusHandler))
                return opusHandler;

            opusHandler = new OpusHandler();
            Handlers.Add(player, opusHandler);
            return opusHandler;
        } 

        public static void Remove(Player player)
        {
            if (Handlers.TryGetValue(player, out OpusHandler opusHandler))
            {
                opusHandler.Decoder.Dispose();
                opusHandler.Encoder.Dispose();

                Handlers.Remove(player);
            }
        }
    }
}*/