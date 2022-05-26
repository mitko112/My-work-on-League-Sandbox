using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore;
using System;
using System.Linq;
using GameServerCore.Enums;

namespace Spells
{
    public class PantheonRJump : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2f,
        };

        IObjAiBase Owner;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
        }

        public void OnSpellCast(ISpell spell)
        {
            AddParticleTarget(Owner, Owner, "Pantheon_Base_R_cas.troy", Owner, flags: 0);

        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            SpellCast(Owner, 1, SpellSlotType.ExtraSlots,spellpos, spellpos, false, Vector2.Zero);
            AddParticle(Owner, null, "Pantheon_Base_R_indicator_green.troy", spellpos, 2.7f);
        }

        public void OnUpdate(float diff)
        {
        }
    }



    public class PantheonRFall : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2f,
        };


        IObjAiBase Owner;
        public ISpellSector DamageSector;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 700f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 2f
            });


        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            AddParticle(Owner, null, "Pantheon_Base_R_aoe_explosion.troy", spellpos);
            Owner.TeleportTo(spellpos.X, spellpos.Y);


            var ap = Owner.Stats.AbilityPower.Total;
            var damage = 100 + (300 * (spell.CastInfo.SpellLevel)) + ap;


            var champs = GetUnitsInRange(spellpos, 700f, true).OrderBy(enemy => Vector2.DistanceSquared(enemy.Position, spellpos)).ToList();
            foreach (var enemy in champs)
            {
                var distance = Vector2.Distance(enemy.Position, spellpos);
                if (distance > 580 && distance <= 700) enemy.TakeDamage(Owner, damage - damage * 0.5f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                if (distance > 464 && distance <= 580) enemy.TakeDamage(Owner, damage - damage * 0.4f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                if (distance > 348 && distance <= 464) enemy.TakeDamage(Owner, damage - damage * 0.3f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                if (distance > 232f && distance <= 348) enemy.TakeDamage(Owner, damage - damage * 0.2f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                if (distance > 116f && distance <= 232f) enemy.TakeDamage(Owner, damage - damage * 0.1f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                if (distance > 0f && distance <= 116f) enemy.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }

        }

        public void OnUpdate(float diff)
        {
        }
    }
}