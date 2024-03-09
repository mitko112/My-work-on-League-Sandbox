using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;



namespace Buffs
{
    public class DariusHemoMarker : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5
        };

        public StatsModifier StatsModifier { get; private set; }
        AttackableUnit Unit;
        float damage;
        float timeSinceLastTick = 900f;
        ObjAIBase owner;
        Particle p;
        Particle p2;
        Particle p3;
        Particle p4;
        Particle p5;
        Particle p6;
        Particle p7;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner as Champion;
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
        public void Blood(Spell spell)
        {
            owner = spell.CastInfo.Owner as Champion;
            AddBuff("DariusHemoVisual", 6.0f, 1, spell, owner, owner);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
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
