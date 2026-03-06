using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
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
    internal class FizzBuffer : IBuffGameScript
    {

        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Buff ThisBuff;
        private Spell Spelll;
        private AttackableUnit Target;
        private ObjAIBase Owner;
        private float ticks = 0;
        private float damage;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
            Spelll = ownerSpell;

            var APratio = Owner.Stats.AbilityPower.Total * 0.2f;
            damage = 24f + (14f * (ownerSpell.CastInfo.SpellLevel - 1)) + APratio;

            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);

            Target = unit;

            ApiEventManager.OnSpellPostCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("FizzJump"), EOnSpellPostCast);
        }

        public void EOnSpellPostCast(Spell spell)
        {
            bool triggeredSpell = true;
            var owner = spell.CastInfo.Owner;
            var oowner = owner as AttackableUnit;
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var startPos = owner.Position;
            var to = trueCoords - startPos;
            if (to.Length() > 400)
            {
                trueCoords = GetPointFromUnit(owner, 400f);
            }
            //StopAnimation(owner, "Spell3a", false);
            //PauseAnimation(owner, true);
            PlayAnimation(owner, "Spell3d", 0.9f);

            owner.SetTargetUnit(null);
            ForceMovement(owner, null, trueCoords, 1200, 0, 0, 0);

            CreateTimer(0.5f, () =>
            {
                AddParticle(owner, null, "Fizz_TrickSlamTwo.troy", trueCoords);
            });

            var buff = owner.GetBuffWithName("FizzTrickSlam");
            buff.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            buff.SetStatusEffect(StatusFlags.Targetable, true);
            buff.SetStatusEffect(StatusFlags.Ghosted, false);
            ApiEventManager.OnSpellPostCast.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;
            if (ticks >= 750.0f)
            {
                if (!Owner.HasBuff("FizzTrickSlam"))
                {
                    var trueCoords = new Vector2(Spelll.CastInfo.TargetPosition.X, Spelll.CastInfo.TargetPosition.Z);
                    var startPos = Owner.Position;
                    var to = trueCoords - startPos;
                    if (to.Length() > 200)
                    {
                        trueCoords = GetPointFromUnit(Owner, 200f);
                    }
                    var units = GetUnitsInRange(trueCoords, 200f, true);
                    for (int i = units.Count - 1; i >= 0; i--)
                    {
                        if (units[i].Team != Spelll.CastInfo.Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret) && units[i] is ObjAIBase ai)
                        {
                            units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                            units.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}