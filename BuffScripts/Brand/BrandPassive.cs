using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;

using GameServerCore.Scripting.CSharp;

using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs

{
    internal class CharScriptBrand: IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase owner;
        AttackableUnit Unit;
        Particle p;
        float damage;
        float timeSinceLastTick = 500f;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            
            damage = unit.Stats.HealthPoints.Total*0.02f;
             p = AddParticleTarget(owner, unit,"BrandFireMark.troy",unit);
            
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000f && !Unit.IsDead && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0;
             
            }
        }
    }
}
