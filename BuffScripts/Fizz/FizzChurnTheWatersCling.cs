using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;


namespace Buffs
{
    internal class FizzChurnTheWatersCling : IBuffGameScript
    {


        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase Owner;
        Spell Spell;
        private AttackableUnit Target;
        private float ticks = 0;
        private float damage;
        private float true1 = 0;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Target = unit;
            Owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            var APratio = Owner.Stats.AbilityPower.Total;
            damage = 200f + (125f * (ownerSpell.CastInfo.SpellLevel - 1)) + APratio;

            StatsModifier.MoveSpeed.PercentBonus -= 0.1f + 0.1f * ownerSpell.CastInfo.SpellLevel;
            unit.AddStatModifier(StatsModifier);

            var units = GetUnitsInRange(Target.Position, 350f, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnPreAttack(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            var owner = Spell.CastInfo.Owner;

            var targetPos = GetPointFromUnit(owner, 1150f);

            ticks += diff;
            if (true1 == 0)
            {
                if (ticks >= 1450.0f && ticks <= 1500.0f)
                {
                    var units = GetUnitsInRange(Target.Position, 350f, true);
                    for (int i = units.Count - 1; i >= 0; i--)
                    {
                        if (units[i].Team != Spell.CastInfo.Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret) && units[i] is ObjAIBase ai)
                        {
                            units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                            var randPoint1 = new Vector2(units[i].Position.X + (40.0f), units[i].Position.Y + 40.0f);

                            var xyz = units[i] as ObjAIBase;
                            xyz.SetTargetUnit(null);

                            ForceMovement(units[i], "", randPoint1, 90.0f, 80.0f, 20.0f, 0.0f, true, ForceMovementType.FURTHEST_WITHIN_RANGE, ForceMovementOrdersType.CANCEL_ORDER, ForceMovementOrdersFacing.KEEP_CURRENT_FACING);
                            units.RemoveAt(i);
                        }
                    }
                    Minion Fish = AddMinion(owner, "FizzShark", "FizzShark", Target.Position);
                    Fish.TakeDamage(Fish.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
                    Fish.PlayAnimation("SPELL4", 1.0f);
                    Fish.SetToRemove();
                    AddParticleTarget(Owner, Target, "Fizz_SharkSplash.troy", Target);
                    AddParticleTarget(Owner, Target, "Fizz_SharkSplash_Ground.troy", Target);
                    var randPoint = new Vector2(Target.Position.X + (40.0f), Target.Position.Y + 40.0f);

                    var xy = Target as ObjAIBase;
                    xy.SetTargetUnit(null);

                    ForceMovement(Target, "", randPoint, 90.0f, 80.0f, 20.0f, 0.0f, true, ForceMovementType.FURTHEST_WITHIN_RANGE, ForceMovementOrdersType.CANCEL_ORDER, ForceMovementOrdersFacing.KEEP_CURRENT_FACING);
                    true1 = 1;
                }
            }
        }
    }
}
