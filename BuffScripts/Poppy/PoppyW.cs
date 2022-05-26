using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class PoppyParagonSpeed : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

            StatsModifier.MoveSpeed.PercentBonus += 0.06f + 0.04f * ownerSpell.CastInfo.SpellLevel; 
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