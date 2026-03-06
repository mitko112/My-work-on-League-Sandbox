using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;



namespace Buffs
{
    class VolibearW : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };
        int StackCount;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            StatsModifier.AttackSpeed.PercentBonus = 0.08f;
                unit.AddStatModifier(StatsModifier);
            StackCount++;
            if (buff.StackCount >= 3)
            {
               SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            for (int i = 1; i < buff.StackCount; i++)
            {
                unit.RemoveStatModifier(StatsModifier);
            }
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}