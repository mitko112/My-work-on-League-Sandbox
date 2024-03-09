using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class Cannibalism : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1

            
        };
        Buff Thisbuff;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();



        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Thisbuff = buff;

            StatsModifier.AttackSpeed.PercentBonus += 0.5f;
            unit.AddStatModifier(StatsModifier);

            StatsModifier.LifeSteal.PercentBonus += 0.5f  + 0.25f * ownerSpell.CastInfo.SpellLevel;
            unit.AddStatModifier(StatsModifier);

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

            Thisbuff.DeactivateBuff();

        }

        public void OnUpdate(float diff)
        {

        }
    }
}