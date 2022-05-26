using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class FizzSeastoneActive : IBuffGameScript
    {
        
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };
        
        private IObjAiBase Owner;
        private float damage;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            var ap = ownerSpell.CastInfo.Owner.Stats.AbilityPower.Total * 0.30f;
            damage = 5f + (ownerSpell.CastInfo.SpellLevel * 5) + ap;
            AddParticleTarget(Owner, Owner, "Fizz_SeastoneTrident.troy", Owner, 5f, bone: "BUFFBONE_GLB_WEAPON_1");
            AddParticleTarget(Owner, Owner, "Fizz_SeastonePassive_Weapon.troy", Owner, bone: "BUFFBONE_GLB_WEAPON_1");

            ApiEventManager.OnHitUnit.AddListener(this, ownerSpell.CastInfo.Owner, TargetTakeDamage, false);
        }

        public void TargetTakeDamage(IDamageData damageData)
        {
            var target = damageData.Target;
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}