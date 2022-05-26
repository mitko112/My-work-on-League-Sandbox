using System.Numerics;
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
    internal class XenZhaoComboAuto : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase Owner;
        ISpell spell;
        IBuff Buff;
        int attackCounter = 1;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (unit is IObjAiBase obj)
            {
                ApiEventManager.OnHitUnit.AddListener(this, obj, OnHitUnit, false);
                obj.CancelAutoAttack(true);
                Owner = obj;
            }
            spell = ownerSpell;
            Buff = buff;
        }
        public void OnHitUnit(IDamageData damageData)
        {
            var target = damageData.Target;
            //From what i saw from old footage, Xin's Q doesnt seem to work with stacks, but with an internal counter
            switch (attackCounter)
            {
                case 1:
                    DealDamage(target);
                    attackCounter++;
                    break;
                case 2:
                    DealDamage(target);
                    attackCounter++;

                    break;
                case 3:
                    DealDamage(target);
                    ForceMovement(target, "RUN", new Vector2(target.Position.X + 10f, target.Position.Y + 10f), 13f, 0, 16.5f, 0);
                    Buff.DeactivateBuff();
                    break;
            }
        }
        public void DealDamage(IAttackableUnit target)
        {
            var ad = Owner.Stats.AttackDamage.Total * 0.2f;
            float damage = 15 * Owner.GetSpell(1).CastInfo.SpellLevel + ad;
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
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