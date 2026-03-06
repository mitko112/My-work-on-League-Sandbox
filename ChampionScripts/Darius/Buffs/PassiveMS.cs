using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    public class DariusHemoMS : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        // ✅ MUST be public
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase _owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = unit as ObjAIBase;
            RecalculateMS();
        }

        public void OnUpdate(float diff)
        {
            // Keep MS synced while buff is active
            RecalculateMS();
        }

        private void RecalculateMS()
        {
            if (_owner == null || _owner.IsDead)
                return;

            // Remove old modifier FIRST
            _owner.RemoveStatModifier(StatsModifier);

            // Count enemy champions that are bleeding
            int bleedingChampions = GetAllChampions()
                .Where(c =>
                    c.Team != _owner.Team &&
                    !c.IsDead &&
                    c.HasBuff("DariusHemoMarker"))
                .Count();

            // Apply 5% MS per champion
            StatsModifier.MoveSpeed.PercentBonus = 0.05f * bleedingChampions;

            // Apply fresh modifier
            _owner.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (_owner == null)
                return;

            // Clean removal → guarantees return to base MS
            _owner.RemoveStatModifier(StatsModifier);
            StatsModifier.MoveSpeed.PercentBonus = 0f;
        }
    }
}