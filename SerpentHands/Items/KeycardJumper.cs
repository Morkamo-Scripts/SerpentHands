using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Items.Keycards;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using UnityEngine;

namespace SerpentHands.Items;

public class KeycardJumper : CustomKeycard
{
    public sealed class SerializableColor
    {
        public byte R { get; set; } = 255;
        public byte G { get; set; } = 255;
        public byte B { get; set; } = 255;
        public byte A { get; set; } = 255;

        public Color32 ToColor() => new(R, G, B, A);
    }
    
    public override uint Id { get; set; } = 6;
    public override string Name { get; set; } = "Ключ-карта Джампера отряда 'Длань Змея'";
    public override string Description { get; set; } = "Ключ-карта Джампера отряда 'Длань Змея'";
    public override float Weight { get; set; } = 1;
    public override SpawnProperties SpawnProperties { get; set; } = null;
    public override ItemType Type { get; set; } = ItemType.KeycardCustomTaskForce;
    public override string KeycardLabel { get; set; } = "SH Jumper";
    
    public SerializableColor KeycardLabelColorRaw { get; set; } = new() { R = 140, G = 0, B = 120, A = 255 };
    public SerializableColor KeycardPermissionsColorRaw { get; set; } = new() { R = 255, G = 180, B = 60, A = 255 };
    public SerializableColor TintColorRaw { get; set; } = new() { R = 140, G = 0, B = 120, A = 255 };

    public override Color32? KeycardLabelColor => KeycardLabelColorRaw?.ToColor();
    public override Color32? KeycardPermissionsColor => KeycardPermissionsColorRaw?.ToColor();
    public override Color32? TintColor => TintColorRaw?.ToColor();
    public override byte Rank { get; set; } = 3;

    public override string KeycardName { get; set; } = "Sh. Jumper";
    public override string SerialNumber { get; set; } = "939106049096173079";

    public override KeycardPermissions Permissions { get; set; } =
        KeycardPermissions.ContainmentLevelTwo |
        KeycardPermissions.ArmoryLevelOne |
        KeycardPermissions.ExitGates |
        KeycardPermissions.Checkpoints |
        KeycardPermissions.Intercom;

    protected override void SetupKeycard(Keycard keycard)
    {
        base.SetupKeycard(keycard);
        keycard.Permissions = Permissions;
    }
}