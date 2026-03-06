using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerLib.GameObjects.AttackableUnits;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Buffs
{
    internal class NasusQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        ObjAIBase Unit;
        Particle p;
        Particle p2;
        Spell spell;
        bool killedtarget = false;
        bool qConsumed;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            spell = ownerSpell;
            thisBuff = buff;
            qConsumed = false;
            ApiEventManager.OnHitUnit.AddListener(owner, owner, ApplyQDamage, true);
            

            StatsModifier.Range.FlatBonus += 50;
            owner.AddStatModifier(StatsModifier);

            owner.CancelAutoAttack(true);

            p = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Nasus_Base_Q_Buf.troy", owner, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1");
            p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Nasus_Base_Q_Wpn_trail.troy", owner, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1");
            owner.SkipNextAutoAttack();
        }

        int pendingStacks = 0;
        bool qHitActive;
        public void ApplyQDamage(DamageData damageData)
        {
            var owner = damageData.Attacker as ObjAIBase;
            var target = damageData.Target;

            if (owner == null || target == null || thisBuff == null || thisBuff.Elapsed())
                return;

            qHitActive = true; // ✅ only mark when Q is VALID

            StopAnimation(owner, "Attack1", true);
            PlayAnimation(owner, "Spell1", 0.8f);
            AddParticleTarget(owner, target, "Nasus_Base_Q_Tar.troy", target);

            float stackDamage = owner.HasBuff("NasusQStacks")
                ? owner.GetBuffWithName("NasusQStacks").StackCount
                : 0f;

            float spellDamage = spell.CastInfo.SpellLevel * 30;
            float finalDamage = spellDamage + stackDamage;

            // 🔴 check kill BEFORE damage
            bool willKill = target.Stats.CurrentHealth <= finalDamage;

            target.TakeDamage(
                owner,
                finalDamage,
                DamageType.DAMAGE_TYPE_PHYSICAL,
                DamageSource.DAMAGE_SOURCE_ATTACK,
                false
            );
            target.TakeDamage(
    owner,
    finalDamage,
    DamageType.DAMAGE_TYPE_PHYSICAL,
    DamageSource.DAMAGE_SOURCE_ATTACK,
    false
);

            // ✅ CHECK REAL RESULT, NOT PREDICTION
            if (target.IsDead)
            {
                int stacks;

                if (target is Champion || target.Stats.HealthPoints.Total >= 1200)
                    stacks = 6;
                else
                    stacks = 3;

                for (int i = 0; i < stacks; i++)
                {
                    AddBuff(
                        "NasusQStacks",
                        25000f,
                        1,
                        spell,
                        owner,
                        owner,
                        true
                    );
                }
            }

            // ✅ ALWAYS deactivate Q after hit
            thisBuff.DeactivateBuff();
        }

        


        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as ObjAIBase;

            for (int i = 0; i < pendingStacks; i++)
            {
                AddBuff("NasusQStacks", 25000f, 1, ownerSpell, owner, owner, true);
            }

            pendingStacks = 0;

            ApiEventManager.OnHitUnit.RemoveListener(owner);
            ApiEventManager.OnKillUnit.RemoveListener(owner);

            RemoveParticle(p);
            RemoveParticle(p2);
        }
    }
}