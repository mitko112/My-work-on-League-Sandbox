using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class Cannibalism : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();



        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

            StatsModifier.AttackSpeed.PercentBonus += 0.5f;
            unit.AddStatModifier(StatsModifier);

            StatsModifier.LifeSteal.PercentBonus += 0.5f  + 0.25f * ownerSpell.CastInfo.SpellLevel;
            unit.AddStatModifier(StatsModifier);

        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

        }

        public void OnUpdate(float diff)
        {

        }
    }
}