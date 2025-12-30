using System;
using System.Linq;
using AdvancedCommands.Commands.JoinWave;
using AdvancedCommands.Components.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using MorkamoEventsRegistrator.Components;
using PlayerRoles;
using SerpentHands.Components;
using SerpentHands.Events;
using SerpentHands.Events.EventArgs.Player;
using SerpentHands.Extensions;
using SerpentHands.Features.Components;
using UnityEngine;
using events = Exiled.Events.Handlers;
using Random = UnityEngine.Random;

namespace SerpentHands.Handlers;

public class RoundHandler : IEventsRegistrator
{
    public void RegisterEvents()
    {
        events.Player.Verified += OnVerifiedPlayer;
        events.Server.RespawningTeam += OnRespawningTeam;
        events.Player.Died += OnDied;
    }

    public void UnregisterEvents()
    {
        events.Player.Verified -= OnVerifiedPlayer;
        events.Server.RespawningTeam -= OnRespawningTeam;
        events.Player.Died -= OnDied;
    }

    public static byte SpawnCount { get; set; }

    private void OnVerifiedPlayer(VerifiedEventArgs ev)
    {
        if (ev.Player.ReferenceHub.gameObject.GetComponent<SerpentHandsProperties>() != null)
            return;

        ev.Player.ReferenceHub.gameObject.AddComponent<SerpentHandsProperties>();

        EventManager.PlayerEvents.InvokePlayerFullConnected(ev.Player);
    }
    
    private void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (ev.Wave.Team != Team.ChaosInsurgency)
            return;

        var players = Player.List
            .Where(pl => pl.Role.Type == RoleTypeId.Spectator)
            .ToList();

        GeneralSettings gs = Plugin.Instance.Config.GeneralSettings;

        if (players.Count < gs.MinPlayers)
            return;
        
        if (players.Count > gs.MaxPlayers)
            players = players.Take(gs.MaxPlayers).ToList();
        
        if (SpawnCount == gs.SpawnLimit/* || Random.Range(1, 101) > gs.SpawnChance*/)
            return;

        AdvancedCommands.Plugin.Instance.IsWaveBlockedAnotherTeam = true;
        
        ev.IsAllowed = false;
        SpawnCount++;
        players.Shuffle();

        var uniqueRoles = Mathf.Min(players.Count, 5);

        for (int i = 0; i < players.Count; i++)
        {
            if (i < uniqueRoles)
            {
                CustomRole.Get((uint)(i + 1))?.AddRole(players[i]);
            }
            else
            {
                CustomRole.Get(5)?.AddRole(players[i]);
            }
        }
        
        AdvancedCommands.Plugin.Instance.LastSpawnTime = DateTime.UtcNow;
        AdvancedCommands.Plugin.Instance.LastSpawnedSquad = SquadTypes.SerpentsHand;
        
        foreach (var player in Player.List)
        {
            player.AdvancedCommand()?.PlayerProperties.HasBeenSpawned = false;
        }

        Timing.CallDelayed(5f, () =>
        {
            AdvancedCommands.Plugin.Instance.IsWaveBlockedAnotherTeam = false;
        });
        
        EventManager.RoundEvents.InvokeSerpentsHandRespawned(players);
    }

    private void OnDied(DiedEventArgs ev)
    {
        if (ev.Attacker?.SerpentHandsProperties()?.SerpentProps.SerpentRole != null)
            ev.Attacker.HumeShield = Mathf.Clamp(ev.Attacker.HumeShield + 25, 0, ev.Attacker.MaxHumeShield);
        
        var serpentProps = ev.Player?.SerpentHandsProperties()?.SerpentProps;
        
        if (serpentProps != null && serpentProps.SerpentRole != null)
        {
            serpentProps.SerpentRole = null;
            Log.SendRaw("Role has NULL", ConsoleColor.Yellow);
        }
    }
}
