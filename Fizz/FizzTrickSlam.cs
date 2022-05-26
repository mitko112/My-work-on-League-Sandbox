using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class FizzTrickSlam : IBuffGameScript
    {
        
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IObjAiBase Owner;
        private ISpell Spell;
        private IAttackableUnit Target;
        private float ticks;
        private float damage;
        private float true1 = 0;
        private float true2 = 1;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            Target = unit;
            Target = unit;
            Owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            var APratio = Owner.Stats.AbilityPower.Total * 0.2f;
            damage = 24f + (14f * (ownerSpell.CastInfo.SpellLevel - 1)) + APratio;
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;
            if (ticks >= 751.0f && ticks <= 801.0f)
            {
                true1 = 1;
                if (true1 == 1)
                {
                    PlayAnimation(Owner, "Spell3c", 0.9f);
                }
            }
            if (ticks > 1100.0f && ticks < 1150.0f)
            {
                var units = GetUnitsInRange(Target.Position, 350f, true);
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    if (units[i].Team != Spell.CastInfo.Owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret) && units[i] is IObjAiBase ai)
                    {
                        units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        AddBuff("FizzESlow", 2f, 1, Spell, units[i], Owner);
                        units.RemoveAt(i);
                    }
                }

                if (true2 == 1)
                {
                    true2 = 2;
                    AddParticleTarget(Owner, Owner, "Fizz_TrickSlam.troy", Owner);
                }
            }
        }
    }
}