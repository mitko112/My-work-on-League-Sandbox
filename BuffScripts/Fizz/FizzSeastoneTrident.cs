using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    internal class FizzSeastoneTrident : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private float timeSinceLastTick;
        private AttackableUnit Unit;
        private float TickingDamage;
        private ObjAIBase Owner;
        private Spell spell;
        private bool limiter = false;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var missinghealth = (unit.Stats.HealthPoints.Total - unit.Stats.CurrentHealth);
            var PercentDamage = (3.5f + (ownerSpell.CastInfo.SpellLevel * 0.5f));
            var AfterDamage = (missinghealth * 0.01f) * PercentDamage;
            var AP = ownerSpell.CastInfo.Owner.Stats.AbilityPower.Total * 0.45f;
            TickingDamage = (30f + AP + AfterDamage) * 0.33f;
            var damage = AP + AfterDamage;
            Unit = unit;
            Owner = owner;
            spell = ownerSpell;
            limiter = true;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 1000.0f)
            {
                Unit.TakeDamage(Owner, TickingDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLPERSIST, false);
                timeSinceLastTick = 0;
            }
        }
    }
}
