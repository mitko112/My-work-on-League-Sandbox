using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class GarenE : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; }

        Champion Owner;
        float damage;
        float TimeSinceLastTick = 500f;
        Particle p;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as Champion;
            var ADratio = owner.Stats.AttackDamage.Total * (0.35f + 0.05f * (ownerSpell.CastInfo.SpellLevel - 1));
            damage = 10f + 12.5f * (ownerSpell.CastInfo.SpellLevel - 1) + ADratio;
            Owner = owner;

            OverrideAnimation(unit, "Spell3", "RUN");
            p = AddParticleTarget(owner, unit, "Garen_Base_E_Spin.troy", unit, buff.Duration);

            SetStatus(unit, StatusFlags.CanAttack, false);
            SetStatus(unit, StatusFlags.Ghosted, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            OverrideAnimation(unit, "RUN", "Spell3");
            if(HasBuff(unit, "GarenQHaste"))
            {
                OverrideAnimation(unit, "Run_Spell1", "RUN");
            }
            RemoveParticle(p);

            SetStatus(unit, StatusFlags.CanAttack, true);
            SetStatus(unit, StatusFlags.Ghosted, false);

            var owner = ownerSpell.CastInfo.Owner;
            Spell spell = owner.SetSpell("GarenE", 2, true);
            spell.SetCooldown(spell.GetCooldown());
        }

        public void OnUpdate(float diff)
        {
            TimeSinceLastTick += diff;
            if (TimeSinceLastTick >= 500.0f)
            {
                PlayAnimation(Owner, "Spell3", 0.0f, 0, 1.0f, AnimationFlags.UniqueOverride);

                var units = GetUnitsInRange(Owner.Position, 330f, true).OrderBy(unit => Vector2.DistanceSquared(unit.Position, unit.Position)).ToList();
                units.RemoveAt(0);
                var isCrit = new Random().Next(0, 100) <= (Owner.Stats.CriticalChance.Total * 100f);
                var tickDamage = damage;
                if(isCrit)
                {
                    tickDamage *= Owner.Stats.CriticalDamage.Total;
                }
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    if (units[i].Team != Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret) && units[i] is ObjAIBase)
                    {
                        var customTickDamage = tickDamage;
                        if (units[i] is Minion)
                        {
                            customTickDamage *= 0.75f;
                        }
                        units[i].TakeDamage(Owner, customTickDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, isCrit);
                    }
                }
                TimeSinceLastTick = 0;
            }
        }
    }
}
