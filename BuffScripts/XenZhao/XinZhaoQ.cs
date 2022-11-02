using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;
using System.Numerics;

namespace Buffs
{
    internal class XenZhaoComboAuto : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase Owner;
        Spell spell;
        Buff Buff;
        int attackCounter = 1;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase obj)
            {
                ApiEventManager.OnHitUnit.AddListener(this, obj, OnHitUnit, false);
                obj.CancelAutoAttack(true);
                Owner = obj;
            }
            spell = ownerSpell;
            Buff = buff;
        }
        public void OnHitUnit(DamageData damageData)
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
        public void DealDamage(AttackableUnit target)
        {
            var ad = Owner.Stats.AttackDamage.Total * 0.2f;
            float damage = 15 * Owner.GetSpell("XenZhaoComboTarget").CastInfo.SpellLevel + ad;
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}