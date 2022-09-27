

using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;

namespace Spells
{

    public class BrandConflagration : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
        };

        

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var APratio = owner.Stats.AbilityPower.Total * 0.55f;
            var damage = 70f * owner.Spells[2].CastInfo.SpellLevel + APratio;
            if (spell.CastInfo.Targets[0].Unit.HasBuff("BrandPassive"))
            {
                var units = GetUnitsInRange(spell.CastInfo.Targets[0].Unit.Position, 300f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor)
                    {
                        continue;
                    }
                    SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, units[i], spell.CastInfo.Targets[0].Unit.Position);
                }
            }
            AddBuff("BrandPassive", 4f, 1, spell, spell.CastInfo.Targets[0].Unit, owner, false);
            spell.CastInfo.Targets[0].Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
        }

      
    }
    public class BrandConflagrationMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var APratio = owner.Stats.AbilityPower.Total * 0.55f;
            var damage = 70 * owner.Spells[2].CastInfo.SpellLevel + APratio;

            AddParticleTarget(owner, target, "BrandConflagration_tar.troy", target, 1f, 1f);
            AddParticleTarget(owner, owner, "BrandConflagration_cas.troy", owner, 1f, 1f);
            AddBuff("BrandPassive", 4f, 1, spell, target, owner, false);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

    }
}
