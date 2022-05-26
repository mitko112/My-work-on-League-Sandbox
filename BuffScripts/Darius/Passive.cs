using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;


namespace Buffs
{
    public class DariusHemoMarker : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5
        };

        public IStatsModifier StatsModifier { get; private set; }
        IAttackableUnit Unit;
        float damage;
        float timeSinceLastTick = 900f;
        IObjAiBase owner;
        IParticle p;
        IParticle p2;
        IParticle p3;
        IParticle p4;
        IParticle p5;
        IParticle p6;
        IParticle p7;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            switch (buff.StackCount)
            {
                case 1:
                    p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_counter_01.troy", unit, buff.Duration);
                    AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_bleed_trail_only1.troy", unit, buff.Duration);
                    damage = (13 + ADratio) / 6f;
                    break;
                case 2:
                    RemoveParticle(p);
                    p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_counter_02.troy", unit, buff.Duration);
                    AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_bleed_trail_only2.troy", unit, buff.Duration);
                    damage = (17 + ADratio) / 6f;
                    break;
                case 3:
                    RemoveParticle(p2);
                    p4 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_counter_03.troy", unit, buff.Duration);
                    AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_bleed_trail_only3.troy", unit, buff.Duration);
                    damage = (21 + ADratio) / 6f;
                    break;
                case 4:
                    RemoveParticle(p4);
                    p5 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_counter_04.troy", unit, buff.Duration);
                    AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_bleed_trail_only4.troy", unit, buff.Duration);
                    damage = (26 + ADratio) / 6f;
                    break;
                case 5:
                    RemoveParticle(p5);
                    p6 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_counter_05.troy", unit, buff.Duration);
                    //AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_mark_for_death_sword.troy", unit, buff.Duration);
                    AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_passive_overhead_max_stack.troy", unit, buff.Duration);
                    //AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_hemo_bleed_indicator_hit.troy", unit, buff.Duration);
                    AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_bleed_trail_only5.troy", unit, buff.Duration);
                    AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "darius_Base_hemo_bleed_trail_only6.troy", unit, buff.Duration);
                    damage = (30 + ADratio) / 6f;
                    Blood(ownerSpell);
                    break;
            }
        }
        public void Blood(ISpell spell)
        {
            owner = spell.CastInfo.Owner as IChampion;
            AddBuff("DariusHemoVisual", 6.0f, 1, spell, owner, owner);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 1000.0f && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0f;
            }
        }
    }
}