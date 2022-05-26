using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs

{
    internal class CassiopeiaPoisonTicker : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.POISON,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IObjAiBase owner;
        private IAttackableUnit Unit;
        private IParticle p;
        private float damage;
        private float timeSinceLastTick = 500f;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            float APratio = owner.Stats.AbilityPower.Total * 0.15f;
            damage = 11.67f + (13.33f * ownerSpell.CastInfo.SpellLevel) + APratio;
            p = AddParticleTarget(owner, unit, "Global_Poison.troy", unit, buff.Duration, bone: "BUFFBONE_GLB_CHANNEL_LOC");
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000f && !Unit.IsDead && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0;
            }
        }
    }
}