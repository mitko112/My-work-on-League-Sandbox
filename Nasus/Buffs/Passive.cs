using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    internal class NasusLifeSteal : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private StatsModifier _statsModifier = new StatsModifier();
        private AttackableUnit _unit;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _unit = unit;
            UpdateLifeSteal();
            unit.AddStatModifier(_statsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            unit.RemoveStatModifier(_statsModifier);
        }

        public void OnUpdate(float diff)
        {
            // Keep checking in case Nasus levels up
            UpdateLifeSteal();
        }

        private void UpdateLifeSteal()
        {
            int level = _unit.Stats.Level;

            // Example scaling: 10% base + 5% every 3 levels
            float lifestealPercent = 0.10f + 0.05f * ((level - 1) / 3);

            _statsModifier.LifeSteal.FlatBonus = lifestealPercent;
        }
    }
}
   