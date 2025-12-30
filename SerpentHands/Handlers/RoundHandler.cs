using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedCommands.Commands.JoinWave;
using AdvancedCommands.Components.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Scp0492;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Scp939;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Patches.Generic;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.Arguments.Scp096Events;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.Arguments.Scp939Events;
using MEC;
using MorkamoEventsRegistrator.Components;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp1507;
using SerpentHands.Components;
using SerpentHands.Events;
using SerpentHands.Events.EventArgs.Player;
using SerpentHands.Extensions;
using SerpentHands.Features.Components;
using UnityEngine;
using events = Exiled.Events.Handlers;
using levents = LabApi.Events.Handlers;
using Random = UnityEngine.Random;

namespace SerpentHands.Handlers;

public class RoundHandler : IEventsRegistrator
{
    public void RegisterEvents()
    {
        events.Player.Verified += OnVerifiedPlayer;
        events.Server.RespawningTeam += OnRespawningTeam;
        events.Player.Died += OnDied;
        events.Player.Hurting += OnHurting;
        events.Player.Spawned += OnPlayerSpawned;
        events.Scp049.Attacking += On049Attack;
        events.Scp106.Attacking += On106Attack;
        events.Scp0492.TriggeringBloodlust += On0492TriggerBloodlust;
        events.Scp939.ValidatingVisibility += On939ValidatingVisibility;
        levents.Scp096Events.AddingTarget += On096AddTarget;
        levents.Scp173Events.AddingObserver += On173AddObserver;
        levents.Scp049Events.UsingSense += On049UsingSense;
    }

    public void UnregisterEvents()
    {
        events.Player.Verified -= OnVerifiedPlayer;
        events.Server.RespawningTeam -= OnRespawningTeam;
        events.Player.Died -= OnDied;
        events.Player.Hurting -= OnHurting;
        events.Player.Spawned -= OnPlayerSpawned;
        events.Scp049.Attacking -= On049Attack;
        events.Scp106.Attacking -= On106Attack;
        events.Scp0492.TriggeringBloodlust -= On0492TriggerBloodlust;
        events.Scp939.ValidatingVisibility -= On939ValidatingVisibility;
        levents.Scp096Events.AddingTarget -= On096AddTarget;
        levents.Scp173Events.AddingObserver -= On173AddObserver;
        levents.Scp049Events.UsingSense -= On049UsingSense;
    }

    public List<Vector3> SpawnPoints =
    [
        new Vector3(18.5f, 292, -42.8f), // Leader
        new Vector3(17.6f, 292, -40.4f), // Eagle
        new Vector3(16.5f, 292, -42.8f), // Initiator
        new Vector3(18.0f, 292, -45.4f), // Jumper
        new Vector3(14.6f, 292, -45.35f), // Support 1
        new Vector3(13.3f, 292, -43.1f), // Support 2
        new Vector3(13.4f, 292, -40.25f), // Support 3
        new Vector3(15.5f, 292, -40.0f) // Support 4
    ];

    public static byte SpawnCount { get; set; }

    private void OnVerifiedPlayer(VerifiedEventArgs ev)
    {
        if (ev.Player.ReferenceHub.gameObject.GetComponent<SerpentHandsProperties>() != null)
            return;

        ev.Player.ReferenceHub.gameObject.AddComponent<SerpentHandsProperties>();

        EventManager.PlayerEvents.InvokePlayerFullConnected(ev.Player);
    }

    private void DelayedTeleport(int i, Player player) 
        => Timing.CallDelayed(1f, () => { player.Teleport(Plugin.Instance.RoundHandler.SpawnPoints[i - 1]); });
    
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
            
            DelayedTeleport(i, players[i]);
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
        }
    }

    private void On096AddTarget(Scp096AddingTargetEventArgs ev)
    {
        if (Player.Get(ev.Target).SerpentHandsProperties().SerpentProps.SerpentRole != null)
            ev.IsAllowed = false;
    }
    
    private void On173AddObserver(Scp173AddingObserverEventArgs ev)
    {
        if (Player.Get(ev.Target).SerpentHandsProperties().SerpentProps.SerpentRole != null)
            ev.IsAllowed = false;
    }
    
    private void On049UsingSense(Scp049UsingSenseEventArgs ev)
    {
        if (Player.Get(ev.Target).SerpentHandsProperties().SerpentProps.SerpentRole != null)
            ev.IsAllowed = false;
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (ev.Attacker?.IsScp == true && ev.Player?.SerpentHandsProperties()?.SerpentProps.SerpentRole != null)
            ev.IsAllowed = false;
        
        if (ev.Attacker?.SerpentHandsProperties()?.SerpentProps.SerpentRole != null && ev.Player?.IsScp == true)
            ev.IsAllowed = false;
    }

    private void OnPlayerSpawned(SpawnedEventArgs ev)
    {
        if (ev.Player?.SerpentHandsProperties()?.SerpentProps.SerpentRole != null)
        {
            if (Scp079Role.TurnedPlayers.Contains(ev.Player))
                Scp079Role.TurnedPlayers.Remove(ev.Player);
        }
    }

    private void On049Attack(AttackingEventArgs ev)
    {
        if (ev.Player?.SerpentHandsProperties()?.SerpentProps.SerpentRole != null)
            ev.IsAllowed = false;
    }
    
    private void On106Attack(Exiled.Events.EventArgs.Scp106.AttackingEventArgs ev)
    {
        if (ev.Player?.SerpentHandsProperties()?.SerpentProps.SerpentRole != null)
            ev.IsAllowed = false;
    }
    
    private void On0492TriggerBloodlust(TriggeringBloodlustEventArgs ev)
    {
        if (ev.Player?.SerpentHandsProperties()?.SerpentProps.SerpentRole != null)
            ev.IsAllowed = false;
    }
    
    private void On939ValidatingVisibility(ValidatingVisibilityEventArgs ev)
    {
        if (Player.Get(ev.Target)?.SerpentHandsProperties()?.SerpentProps.SerpentRole != null)
        {
            ev.IsLateSeen = true;
            ev.IsAllowed = true;
        }
    }
}
