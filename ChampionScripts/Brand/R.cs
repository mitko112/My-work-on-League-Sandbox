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
using System;
using System.Buffers;
using System.Linq;

namespace Spells
{
    public class BrandWildfire : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = false,
            SpellFXOverrideSkins = new string[] { "FrostFireBrand" },
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
            var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 150 + AP;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("BrandPassive", 5f, 1, spell, target, owner);

            var units = GetUnitsInRange(target.Position, 750.0f, true).FindAll(unit => unit != target && unit.Team != owner.Team && !(unit is ObjBuilding || unit is BaseTurret) && !unit.HasBuff("BrandPassive") && !unit.IsDead).OrderBy(unit => Vector2.DistanceSquared(unit.Position, target.Position)).ToList();
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, units[0], target.Position);
            AddBuff("BrandPassive", 5f, 1, spell, units[0], owner);


        }


    }

    public class BrandWildfireMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = false,
            PersistsThroughDeath = true,
            SpellFXOverrideSkins = new string[] { "FrostFireBrand" },
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            LogDebug("Is this shit activated at least from missile?");
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {

        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var AP = owner. Stats.AbilityPower.Total*0.5f;
            var damage = 150 + AP;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("BrandPassive", 5f, 1, spell, target, owner);
            AddBuff("BrandWildfire", 7f, 1, spell, owner, owner);

            var units = GetUnitsInRange(target.Position, 750.0f, true)
                .FindAll(unit => unit != target && unit.Team != owner.Team && !(unit is ObjBuilding || unit is BaseTurret) && !unit.IsDead)
                .OrderBy(unit => unit.HasBuff("BrandPassive") && unit is Champion ? 0 : 1) // Prioritize units with "BrandPassive" buff
                .ToList();

            foreach (var unit in units)
            {
                if (owner.GetBuffWithName("BrandWildfire").StackCount < 5)
                {

                    SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, unit, target.Position);
                    LogDebug("Hey, I hit someone from Missile");
                    break;
                }
            }
        }
    }


}