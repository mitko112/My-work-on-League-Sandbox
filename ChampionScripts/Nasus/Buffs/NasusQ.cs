
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
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            spell = ownerSpell;
            thisBuff = buff;

            ApiEventManager.OnHitUnit.AddListener(owner, owner, ApplyQDamage, true);
            ApiEventManager.OnKillUnit.AddListener(owner, owner, ApplyQStacks, true);

            StatsModifier.Range.FlatBonus += 50;
            owner.AddStatModifier(StatsModifier);

            owner.CancelAutoAttack(true);

            p = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Nasus_Base_Q_Buf.troy", owner, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1");
            p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Nasus_Base_Q_Wpn_trail.troy", owner, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1");
            owner.SkipNextAutoAttack();
        }


        public void ApplyQDamage(DamageData damageData)
        {
            var owner = damageData.Attacker;
            var target = damageData.Target;

            StopAnimation(owner, "Attack1", true);
            PlayAnimation(owner, "Spell1", 0.8f);
            AddParticleTarget(owner, target, "Nasus_Base_Q_Tar.troy", target);

            if (!thisBuff.Elapsed() && thisBuff != null && target != null)
            {
                if (owner.HasBuff("NasusQStacks"))
                {
                    float StackDamage = owner.GetBuffWithName("NasusQStacks").StackCount;
                    
                    float spellDamage = spell.CastInfo.SpellLevel * 30;
                    float finaldamage = StackDamage  + spellDamage;
                    target.TakeDamage(owner, finaldamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    thisBuff.DeactivateBuff();
                }
                else
                {
                    
                    float spellDamage = spell.CastInfo.SpellLevel * 30;
                    float finaldamage =   spellDamage;
                    target.TakeDamage(owner, finaldamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    thisBuff.DeactivateBuff();
                }
            }
        }

        public void ApplyQStacks(DeathData deathData)
        {
            var owner = deathData.Killer;
            var target = deathData.Unit;
            var Owner = owner as ObjAIBase;
            killedtarget = true;


            if (killedtarget == true)
            {
                thisBuff.DeactivateBuff();

                AddBuff("NasusQStacks", 25000f, 1, spell, owner, Owner, true);
                AddBuff("NasusQStacks", 25000f, 1, spell, owner, Owner, true);
                AddBuff("NasusQStacks", 25000f, 1, spell, owner, Owner, true);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            RemoveParticle(p);
            RemoveParticle(p2);
            //ApiEventManager.OnHitUnit.RemoveListener(owner);
            //ApiEventManager.OnKillUnit.RemoveListener(owner);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
