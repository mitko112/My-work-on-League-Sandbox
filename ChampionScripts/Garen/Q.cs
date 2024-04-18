using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using static LeaguePackets.Game.Common.CastInfo;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.Logging;

namespace Spells
{
    public class GarenQ : ISpellScript
    {

        public static Spell BASIC_AUTO_ATTACK_SPELL = null;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if(BASIC_AUTO_ATTACK_SPELL == null)
            {
                BASIC_AUTO_ATTACK_SPELL = owner.AutoAttackSpell;
            }

            float hasteDuration = 1.5f + 0.75f * (owner.GetSpell("GarenQ").CastInfo.SpellLevel - 1);
            AddBuff("GarenQ", 4.5f, 1, spell, owner, owner);
            AddBuff("GarenQHaste", hasteDuration, 1, spell, owner, owner);
        }

        public void OnSpellPostCast(Spell spell)
        {
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
            spell.SetCooldown(0);
        }
    }

    public class GarenQAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            CastTime = 0.25f
        };

        private ObjAIBase owner;
        private AttackableUnit target;
        private Spell spell;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            this.owner = owner;
            this.target = target;
            this.spell = spell;

            LoggerProvider.GetLogger().Warn("SpellPreCast");
        }

        public void OnSpellCast(Spell spell)
        {
            LoggerProvider.GetLogger().Warn("SpellCast");
        }

        public void OnSpellPostCast(Spell spell)
        {
            LoggerProvider.GetLogger().Warn("SpellPostCast");
            AttackableUnit currentTarget = spell.CastInfo.Owner.TargetUnit;
            if (currentTarget == null || currentTarget.IsDead)
            {
                OnSpellEnd();
                return;
            }

            AddParticleTarget(owner, null, "Garen_Base_Q_Land.troy", owner);

            SilenceTarget();
            DealSpellDamage();

            OnSpellEnd();
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            LoggerProvider.GetLogger().Warn("OnDeactivate");
            OnSpellEnd();
        }

        private void SilenceTarget()
        {
            float silenceDuration = 1.5f + 0.25f * (owner.GetSpell("GarenQ").CastInfo.SpellLevel - 1);
            AddBuff("Silence", silenceDuration, 1, spell, target, owner);
            AddParticleTarget(owner, target, "Garen_Base_Q_Silence.troy", target, silenceDuration, bone: "head");
        }

        private void DealSpellDamage()
        {
            var spellLevel = owner.GetSpell("GarenQ").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 1.4f;
            var damage = 30 + (25 * (spellLevel - 1)) + ADratio;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        private void OnSpellEnd()
        {
            LoggerProvider.GetLogger().Warn("Spell End");
            owner.SetAutoAttackSpell(GarenQ.BASIC_AUTO_ATTACK_SPELL, false);
            if(owner.HasBuff("GarenQ"))
            {
                owner.GetBuffWithName("GarenQ").DeactivateBuff();
            }
        }
    }
}
