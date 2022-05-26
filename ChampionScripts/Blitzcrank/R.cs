using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Collections.Generic;

namespace Spells
{
    public class StaticField : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit unit, ISpellMissile mis, ISpellSector sector)
        {
            var Owner = spell.CastInfo.Owner;
            float AP = Owner.Stats.AbilityPower.Total;
            float damage = 125 + (125 * spell.CastInfo.SpellLevel) + AP;

            unit.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public ISpellSector DRMundoWAOE;
        public void OnSpellPostCast(ISpell spell)
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

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}

