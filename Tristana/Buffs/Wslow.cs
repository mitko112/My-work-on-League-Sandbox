using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    public class TristanaWSlow : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        AttackableUnit Unit;
        public StatsModifier StatsModifier { get; private set; } =
            new StatsModifier();

        public void OnActivate(
            AttackableUnit unit,
            Buff buff,
            Spell ownerSpell)
        {
            Unit = unit;

            // 60% slow
            StatsModifier.MoveSpeed.PercentBonus = -0.6f;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(
            AttackableUnit unit,
            Buff buff,
            Spell ownerSpell)
        {
            unit.RemoveStatModifier(StatsModifier);
        }

        public void OnUpdate(float diff) { }
    }
}