using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;

namespace Spells
{
    public class StaticField : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }


        public void TargetExecute(Spell spell, AttackableUnit unit, SpellMissile mis, SpellSector sector)
        {
            var Owner = spell.CastInfo.Owner;
            float AP = Owner.Stats.AbilityPower.Total;
            float damage = 125 + (125 * spell.CastInfo.SpellLevel) + AP;

            unit.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public SpellSector DRMundoWAOE;
        public void OnSpellPostCast(Spell spell)
        {
            DRMundoWAOE = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = spell.CastInfo.Owner,
                Length = 600f,
                Tickrate = 500,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
            var p = AddParticleTarget(spell.CastInfo.Owner, spell.CastInfo.Owner, "StaticField_tar.troy", spell.CastInfo.Owner, 1.0f);
            var p2 = AddParticleTarget(spell.CastInfo.Owner, spell.CastInfo.Owner, "StaticField_nova.troy", spell.CastInfo.Owner, 1.0f);
            CreateTimer(0.1f, () => { DRMundoWAOE.SetToRemove(); });
        }

        
    }
}
