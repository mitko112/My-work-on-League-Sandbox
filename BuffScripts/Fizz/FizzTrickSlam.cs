using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    internal class FizzTrickSlam : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase Owner;
        private Spell Spell;
        private AttackableUnit Target;
        private float ticks;
        private float damage;
        private float true1 = 0;
        private float true2 = 1;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            Target = unit;
            Target = unit;
            Owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            var APratio = Owner.Stats.AbilityPower.Total * 0.2f;
            damage = 24f + (14f * (ownerSpell.CastInfo.SpellLevel - 1)) + APratio;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnPreAttack(Spell spell)
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
                    if (units[i].Team != Spell.CastInfo.Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret) && units[i] is ObjAIBase ai)
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
