using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{

    internal class MalphiteCleave : IBuffGameScript
    {


        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1,
            IsHidden = false
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        AttackableUnit Unit;
        Spell Spell;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            ApiEventManager.OnHitUnit.AddListener(this, owner, ApplySplashDamage, false);

        }

        public void ApplySplashDamage(DamageData damageData)
        {
            var owner = damageData.Attacker;
            var target = damageData.Target;
            var splashDamage = owner.Stats.AttackDamage.Total * (0.22f + 0.08f * Spell.CastInfo.SpellLevel);
            var referencePoint = GetPointFromUnit(owner, 200f);
            var enemies = GetUnitsInRange(referencePoint, 200f, true);

            if (!owner.HasBuff("ObduracyBuff"))
            {
                AddParticleTarget(owner, target, "Malphite_Base_CleaveHit.troy", target);
            }
            else
            {
                AddParticleTarget(owner, target, "Malphite_Base_CleaveEnragedHit.troy", target);
            }

            foreach (var enemy in enemies)
            {
                if (enemy != target && enemy != owner)
                {
                    enemy.TakeDamage(owner, splashDamage, DamageType.DAMAGE_TYPE_PHYSICAL,
                        DamageSource.DAMAGE_SOURCE_SPELLAOE, false);  
                }
            }

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            
        }

    }
}