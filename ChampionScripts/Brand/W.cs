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
    public class BrandFissure : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
            SpellToggleSlot = 4
        };
        public SpellSector DamageSector;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            var pre = AddParticle(owner, null, "BrandPOF_charge.troy", spellPos, lifetime: 1.5f);
            AddParticle(owner, null, "BrandPOF_tar_green.troy", spellPos, 1f);
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Tickrate = 1,
                Length = 250f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
            CreateTimer(1.0f, () => { var pre = AddParticle(owner, null, "BrandPOF_tar.troy", spellPos, lifetime: 4.0f); DamageSector.SetToRemove(); });
        }

      
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            //Graves_SmokeGrenade_Cloud_Team_Green.troy
            //Graves_SmokeGrenade_Cloud_Team_Red.troy
            var owner = spell.CastInfo.Owner;
            if (target.HasBuff("BrandPassive"))
            {
                var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.75f;
                var damage = 37.5f + (56.25f * spell.CastInfo.SpellLevel) + ap;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddBuff("BrandPassive", 4f, 1, spell, target, owner);
            }
            else
            {
                var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.6f;
                var damage = 30 + (45 * spell.CastInfo.SpellLevel) + ap;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddBuff("BrandPassive", 4f, 1, spell, target, owner);
            }
        }

       

    }
}
