using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class VayneSilveredBolts : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };
        public IStatsModifier StatsModifier { get; private set; }

        IParticle p;
        IAttackableUnit Unit;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Unit = unit;
            switch (buff.StackCount)
            {
                case 1:
                    p = AddParticleTarget(ownerSpell.CastInfo.Owner, null, "vayne_W_ring1.troy", unit, float.MaxValue);
                    break;
                case 2:
                    RemoveParticle(p);
                    p = AddParticleTarget(ownerSpell.CastInfo.Owner, null, "vayne_W_ring2.troy", unit, float.MaxValue);
                    break;
                case 3:
                    AddParticleTarget(ownerSpell.CastInfo.Owner, null, "vayne_W_tar.troy", unit);
                    TargetTakeDamage(ownerSpell);
                    buff.DeactivateBuff();
                    break;
            }
        }
        public void TargetTakeDamage(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            float percentHealthDMG = Unit.Stats.HealthPoints.Total * (0.04f + 0.1f * (owner.GetSpell(1).CastInfo.SpellLevel - 1));
            float flatDMG = 20 + 10f * (owner.GetSpell(1).CastInfo.SpellLevel - 1);
            float damage = flatDMG + percentHealthDMG;

            Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}