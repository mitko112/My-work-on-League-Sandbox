using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;

namespace Spells
{
    public class Rupture : ISpellScript
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

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            var pre = AddParticle(owner, null, "rupture_cas_01.troy", spellPos, lifetime: 1.5f);
            AddParticle(owner, null, "rupture_cas_01_green_team.troy", spellPos, 1f);
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Tickrate = 1,
                Length = 175f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
            CreateTimer(1f, () => { var pre = AddParticle(owner, null, "rupture_cas_02.troy", spellPos, lifetime: 4.0f); DamageSector.SetToRemove(); });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
           
            var owner = spell.CastInfo.Owner;
            
            
             
            {
                var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total;
                var damage = 80*spell.CastInfo.SpellLevel + ap;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddBuff("RuptureTarget", 1f, 1, spell, target, owner);
                ForceMovement(target, "RUN", new Vector2(target.Position.X + 10f, target.Position.Y + 10f), 13f, 0, 16.5f, 0);
            }
        }
    }
}
