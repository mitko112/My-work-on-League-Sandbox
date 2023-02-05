using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs

{
    internal class InfiniteDuress: IBuffGameScript
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
        float timeSinceLastTick = 150f;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            float ADratio = owner.Stats.AttackDamage.FlatBonus *0.4f;
            damage = 50 * ownerSpell.CastInfo.SpellLevel + ADratio;
            SetStatus(Unit, StatusFlags.CanMove, false);
            SetStatus(Unit, StatusFlags.CanAttack, false);
            SetStatus(Unit, StatusFlags.CanCast, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 350f && !Unit.IsDead && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0;
                SetStatus(Unit, StatusFlags.CanMove, true);
                SetStatus(Unit, StatusFlags.CanAttack, true);
                SetStatus(Unit, StatusFlags.CanCast, true);  

            }
        }
    }
}