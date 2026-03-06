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
    internal class InfiniteDuress : IBuffGameScript
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

        float damage;
        float timeSinceLastTick = 0f;
        const float TICK_TIME = 350f;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            Unit = unit;

            float ADratio = owner.Stats.AttackDamage.FlatBonus * 0.4f;
            damage = 50 * ownerSpell.CastInfo.SpellLevel + ADratio;

            // Suppress target
            SetStatus(Unit, StatusFlags.CanMove, false);
            SetStatus(Unit, StatusFlags.CanAttack, false);
            SetStatus(Unit, StatusFlags.CanCast, false);

            // Lock Warwick
            SetStatus(owner, StatusFlags.CanMove, false);
            SetStatus(owner, StatusFlags.CanAttack, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            // Restore target
            SetStatus(Unit, StatusFlags.CanMove, true);
            SetStatus(Unit, StatusFlags.CanAttack, true);
            SetStatus(Unit, StatusFlags.CanCast, true);

            // Restore Warwick
            SetStatus(owner, StatusFlags.CanMove, true);
            SetStatus(owner, StatusFlags.CanAttack, true);
        }

        public void OnUpdate(float diff)
        {
            if (Unit == null || Unit.IsDead || owner.IsDead)
                return;

            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 350f)
            {
                Unit.TakeDamage(
                    owner,
                    damage,
                    DamageType.DAMAGE_TYPE_MAGICAL,          // IMPORTANT
                    DamageSource.DAMAGE_SOURCE_ATTACK,        // THIS is the trick
                    false
                );

                timeSinceLastTick = 0f;
            }
        }
    }
    }
