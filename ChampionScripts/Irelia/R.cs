using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class IreliaTranscendentBlades : ISpellScript
    {
        ObjAIBase Irelia;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellCast(Spell spell)
        {
            Irelia = spell.CastInfo.Owner as Champion;
            AddBuff("IreliaTranscendentBlades", 10f, 1, spell, Irelia, Irelia);
        }
    }
    public class IreliaTranscendentBladesSpell : ISpellScript
    {
        float Damage;
        ObjAIBase Irelia;
        SpellMissile Blade;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Irelia = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Blade = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Damage = 40 + (40 * spell.CastInfo.SpellLevel) + (Irelia.Stats.AbilityPower.Total * 0.5f) + (Irelia.Stats.AttackDamage.FlatBonus * 0.7f);
            target.TakeDamage(Irelia, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            //AddParticleTarget(Irelia, Irelia, "irelia_ult_cas.troy", Irelia, lifetime: 1f);
            AddParticleTarget(Irelia, target, "irelia_ult_tar.troy", Irelia, lifetime: 1f);
            if (Damage > 0) { Irelia.Stats.CurrentHealth += Damage * 0.25f; }
        }
    }
}