

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

    internal class Feast : IBuffGameScript
    {


        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_OVERLAPS,
            MaxStacks = 6
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        AttackableUnit Unit;
        Spell Spell;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;
            Unit = unit;
            var owner = ownerSpell.CastInfo.Owner;
            thisBuff = buff;
            ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, false);

            StatsModifier.Size.PercentBonus += 0.2f;
            StatsModifier.Range.FlatBonus = 3.8f * ownerSpell.CastInfo.SpellLevel;
            var HealthBuff = 90f * ownerSpell.CastInfo.SpellLevel;
            StatsModifier.HealthPoints.BaseBonus += HealthBuff;

            unit.AddStatModifier(StatsModifier);
            if (!owner.IsDead) unit.Stats.CurrentHealth += HealthBuff;

        }
        public void OnDeath(DeathData data)
        {
            var stackCount = thisBuff.StackCount;
            var half = stackCount / 2;
            var difference = stackCount - half;
            var buffList = data.Unit.GetBuffsWithName("Feast");
            foreach (Buff rBuff in buffList)
            {
                rBuff.DeactivateBuff();
            }

            if (stackCount < 2)
            {
                thisBuff.DeactivateBuff();
            }
            else
            {
                for (int i = 1; i <= difference; i++)
                {
                    AddBuff("Feast", 1f, 1, Spell, data.Unit, data.Unit as ObjAIBase, true);
                }
            }

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

        }

    }
}

















